param([Parameter(Mandatory = $true)] [string] $PdfPath)
$info = Get-Item $PdfPath
Write-Host "Name           : $($info.Name)"
Write-Host "Size           : $($info.Length) bytes"
Write-Host "LastWriteTime  : $($info.LastWriteTime)"
$bytes = [System.IO.File]::ReadAllBytes($PdfPath)
$header = [System.Text.Encoding]::ASCII.GetString($bytes, 0, [Math]::Min(8, $bytes.Length))
$tail   = [System.Text.Encoding]::ASCII.GetString($bytes, [Math]::Max(0, $bytes.Length - 16), [Math]::Min(16, $bytes.Length))
Write-Host "PDF header     : $header"
Write-Host "PDF tail       : $tail"
$validHeader = $header.StartsWith('%PDF-')
$validTail   = $tail.Contains('%%EOF')
Write-Host "Header valid   : $validHeader"
Write-Host "Tail valid     : $validTail"
