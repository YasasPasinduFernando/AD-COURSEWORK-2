param(
    [Parameter(Mandatory = $true)] [string] $DocxPath,
    [Parameter(Mandatory = $true)] [string] $PdfPath
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

    Write-Host "Updating Tables of Contents..."
    try {
        foreach ($toc in $doc.TablesOfContents) {
            $toc.Update() | Out-Null
        }
        Write-Host "  Tables of Contents updated: $($doc.TablesOfContents.Count)"
    } catch {
        Write-Host "  Warning: TOC update failed: $_"
    }

    try {
        $doc.Save() | Out-Null
    } catch {
        Write-Host "  Warning: Save after TOC update failed: $_"
    }

    try {
        $pageCount = $doc.ComputeStatistics(2)
        $wordCount = $doc.ComputeStatistics(0)
        Write-Host "Page count  : $pageCount"
        Write-Host "Word count  : $wordCount"
    } catch {
        Write-Host "Statistics warning: $_"
    }

    $doc.SaveAs2([ref]$pdfFull, [ref]17)
    $doc.Close([ref]$false)
}
finally {
    if ($word -ne $null) {
        $word.Quit()
        [System.Runtime.InteropServices.Marshal]::ReleaseComObject($word) | Out-Null
    }
}
