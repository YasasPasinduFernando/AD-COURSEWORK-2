param(
    [Parameter(Mandatory = $true)] [string] $DocxPath,
    [Parameter(Mandatory = $true)] [string] $PdfPath,
    [switch] $UpdateFields
)

$ErrorActionPreference = 'Stop'

$docxFull = (Resolve-Path $DocxPath).Path
$pdfFull  = [System.IO.Path]::GetFullPath($PdfPath)

Write-Host "Source DOCX : $docxFull"
Write-Host "Target PDF  : $pdfFull"

$word = $null
try {
    $word = New-Object -ComObject Word.Application
    $word.Visible = $false
    $word.DisplayAlerts = 0
    $word.Options.UpdateFieldsAtPrint = $false
    $word.Options.UpdateLinksAtOpen = $false

    $doc = $word.Documents.Open($docxFull, [ref]$false, [ref]$true)

    if ($UpdateFields) {
        try {
            foreach ($toc in $doc.TablesOfContents) { $null = $toc.Update() }
            foreach ($tof in $doc.TablesOfFigures) { $null = $tof.Update() }
            foreach ($field in $doc.Fields) { $null = $field.Update() }
        } catch {
            Write-Host "Field update warning: $_"
        }
    }

    try {
        $pageCount = $doc.ComputeStatistics(2)  # wdStatisticPages
        $wordCount = $doc.ComputeStatistics(0)  # wdStatisticWords
        Write-Host "Page count  : $pageCount"
        Write-Host "Word count  : $wordCount"
    } catch {
        Write-Host "Statistics warning: $_"
    }

    $doc.SaveAs2([ref]$pdfFull, [ref]17)  # wdFormatPDF
    $doc.Close([ref]$false)
}
finally {
    if ($word -ne $null) {
        $word.Quit()
        [System.Runtime.InteropServices.Marshal]::ReleaseComObject($word) | Out-Null
    }
}
