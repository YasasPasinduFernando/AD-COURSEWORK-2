using System.Text;
using AD_COURSEWORK_2.Infrastructure;
using FluentAssertions;
using Xunit;

namespace UniManage.Tests;

public class CsvWriterTests
{
    [Fact]
    public void Build_includes_utf8_bom_and_header_row()
    {
        var headers = new[] { "ColA", "ColB" };
        var rows = new List<object?[]>
        {
            new object?[] { 1, "x" }
        };

        var bytes = CsvWriter.Build(headers, rows);

        bytes.Length.Should().BeGreaterThan(3);
        bytes[0].Should().Be(0xEF);
        bytes[1].Should().Be(0xBB);
        bytes[2].Should().Be(0xBF);

        var text = Encoding.UTF8.GetString(bytes);
        text.Should().Contain("ColA");
        text.Should().Contain("ColB");
        var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var headerLine = lines[0].TrimStart('\uFEFF');
        headerLine.Should().Be("ColA,ColB");
        lines.Should().Contain(l => l == "1,x");
    }
}
