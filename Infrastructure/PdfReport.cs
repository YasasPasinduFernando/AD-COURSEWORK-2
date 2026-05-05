using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AD_COURSEWORK_2.Infrastructure;

/// <summary>
/// Branded PDF report generator. Each report is a single document with
/// a UniManage cover header (gradient bar, brand mark, eyebrow, title,
/// subtitle), summary chip row, table of data, and a page footer.
/// </summary>
public static class PdfReport
{
    private const string BrandName = "UniManage";
    private const string BrandTag = "University Course Management System";
    private const string ColorPrimaryDark = "#1e3a8a";
    private const string ColorPrimary = "#2563eb";
    private const string ColorAccent = "#22d3ee";
    private const string ColorText = "#0b1220";
    private const string ColorMuted = "#64748b";
    private const string ColorBorder = "#e5ecf5";
    private const string ColorSurface = "#f6f8fc";
    private const string ColorHeader = "#1e3a8a";
    private const string ColorRowAlt = "#f8fafc";

    public sealed record Chip(string Label, string Value, string? Tone = null);

    public sealed class TableSpec
    {
        public required string[] Headers { get; init; }
        public required IReadOnlyList<object?[]> Rows { get; init; }
        public float[]? RelativeWidths { get; init; }
        public bool[]? RightAlign { get; init; }
    }

    public static byte[] Build(
        string title,
        string subtitle,
        IEnumerable<Chip> chips,
        TableSpec table,
        string? footerNote = null)
    {
        var chipList = chips.ToList();

        return Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(0);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(t => t
                    .FontFamily(Fonts.Calibri)
                    .FontSize(10)
                    .FontColor(ColorText));

                page.Header().Element(c => RenderHeader(c, title, subtitle));

                page.Content().PaddingHorizontal(28).PaddingVertical(18).Column(col =>
                {
                    col.Spacing(14);
                    if (chipList.Count > 0)
                        col.Item().Element(e => RenderChips(e, chipList));
                    col.Item().Element(e => RenderTable(e, table));
                    if (!string.IsNullOrWhiteSpace(footerNote))
                        col.Item().PaddingTop(6).Text(footerNote)
                            .FontSize(8.5f).FontColor(ColorMuted).Italic();
                });

                page.Footer().Element(RenderFooter);
            });
        }).GeneratePdf();
    }

    private static void RenderHeader(IContainer container, string title, string subtitle)
    {
        container.Background(ColorPrimaryDark).Padding(28).Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("UNIMANAGE REPORT")
                    .FontSize(9).FontColor("#cfe1ff").Bold().LetterSpacing(2);
                col.Item().PaddingTop(4).Text(title)
                    .FontSize(20).FontColor(Colors.White).Bold();
                col.Item().PaddingTop(2).Text(subtitle)
                    .FontSize(10.5f).FontColor("#cfe1ff");
            });
            row.ConstantItem(160).AlignRight().AlignMiddle().Column(col =>
            {
                col.Item().AlignRight().Text(BrandName)
                    .FontSize(13).FontColor(Colors.White).Bold();
                col.Item().AlignRight().Text(BrandTag)
                    .FontSize(8.5f).FontColor("#cfe1ff");
                col.Item().PaddingTop(6).AlignRight().Text(DateTime.Now.ToString("dddd, MMM d yyyy · HH:mm"))
                    .FontSize(8).FontColor("#a5c2f4");
            });
        });
    }

    private static void RenderChips(IContainer container, List<Chip> chips)
    {
        container.Row(row =>
        {
            foreach (var chip in chips)
            {
                row.RelativeItem().PaddingRight(8).Background(ColorSurface)
                    .Border(1).BorderColor(ColorBorder)
                    .Padding(10).Column(col =>
                    {
                        col.Item().Text(chip.Label.ToUpperInvariant())
                            .FontSize(7.5f).FontColor(ColorMuted).Bold().LetterSpacing(1.2f);
                        col.Item().PaddingTop(2).Text(chip.Value)
                            .FontSize(15).FontColor(ColorText).Bold();
                    });
            }
        });
    }

    private static void RenderTable(IContainer container, TableSpec spec)
    {
        var widths = spec.RelativeWidths ?? Enumerable.Repeat(1f, spec.Headers.Length).ToArray();
        var right = spec.RightAlign ?? new bool[spec.Headers.Length];

        container.Border(1).BorderColor(ColorBorder).Table(table =>
        {
            table.ColumnsDefinition(c =>
            {
                foreach (var w in widths)
                    c.RelativeColumn(w);
            });

            table.Header(header =>
            {
                for (var i = 0; i < spec.Headers.Length; i++)
                {
                    var cell = header.Cell().Background(ColorHeader)
                        .PaddingVertical(8).PaddingHorizontal(10);
                    var aligned = right[i]
                        ? cell.AlignRight().Text(spec.Headers[i])
                        : cell.Text(spec.Headers[i]);
                    aligned.FontColor(Colors.White).Bold().FontSize(9.5f);
                }
            });

            for (var r = 0; r < spec.Rows.Count; r++)
            {
                var row = spec.Rows[r];
                string bg = r % 2 == 0 ? "#ffffff" : ColorRowAlt;
                for (var i = 0; i < spec.Headers.Length; i++)
                {
                    var raw = i < row.Length ? row[i] : null;
                    var text = FormatCell(raw);
                    var cell = table.Cell().Background(bg)
                        .BorderTop(0.5f).BorderColor(ColorBorder)
                        .PaddingVertical(7).PaddingHorizontal(10);
                    var aligned = right[i] ? cell.AlignRight().Text(text) : cell.Text(text);
                    aligned.FontSize(9.5f).FontColor(ColorText);
                }
            }

            if (spec.Rows.Count == 0)
            {
                table.Cell().ColumnSpan((uint)spec.Headers.Length)
                    .Background(Colors.White)
                    .PaddingVertical(28).AlignCenter()
                    .Text("No data available for this report.")
                    .FontColor(ColorMuted).Italic();
            }
        });
    }

    private static string FormatCell(object? value)
    {
        return value switch
        {
            null => "—",
            DateTime dt => dt.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
            DateOnly d => d.ToString("yyyy-MM-dd"),
            decimal m => m.ToString("0.##"),
            double d => d.ToString("0.##"),
            float f => f.ToString("0.##"),
            _ => value.ToString() ?? "—"
        };
    }

    private static void RenderFooter(IContainer container)
    {
        container.BorderTop(0.75f).BorderColor(ColorBorder)
            .PaddingHorizontal(28).PaddingVertical(8)
            .Row(row =>
            {
                row.RelativeItem().Text(t =>
                {
                    t.Span($"© {DateTime.UtcNow.Year} {BrandName} · {BrandTag}")
                        .FontSize(8).FontColor(ColorMuted);
                });
                row.ConstantItem(120).AlignRight().Text(t =>
                {
                    t.Span("Page ").FontSize(8).FontColor(ColorMuted);
                    t.CurrentPageNumber().FontSize(8).FontColor(ColorPrimary).Bold();
                    t.Span(" of ").FontSize(8).FontColor(ColorMuted);
                    t.TotalPages().FontSize(8).FontColor(ColorPrimary).Bold();
                });
            });
    }
}
