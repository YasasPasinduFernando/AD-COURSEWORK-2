param(
    [string[]] $Files = @(
        'documentation_application_development/Main_Application_Development_Report.md',
        'documentation_application_development/Expanded_Individual_Contribution.md',
        'documentation_application_development/User_Manual_Application_Development.md'
    )
)

foreach ($f in $Files) {
    $c = Get-Content $f -Raw
    $sr = ([regex]::Matches($c, '\[SCREENSHOT REQUIRED\]')).Count
    $de = ([regex]::Matches($c, '\[DIAGRAM EXPORT REQUIRED\]')).Count
    $nc = ([regex]::Matches($c, '\[NEEDS CONFIRMATION\]')).Count
    $vf = ([regex]::Matches($c, '\[VERIFY\]')).Count
    $ii = ([regex]::Matches($c, '\[INSERT IF REQUIRED\]')).Count
    $id = ([regex]::Matches($c, '\[INSERT DATE\]')).Count
    $em = ([regex]::Matches($c, '[\u2013\u2014]')).Count
    $we = ([regex]::Matches($c, '\bwe\b|\bWe\b|\bour\b|\bOur\b')).Count
    $name = Split-Path $f -Leaf
    Write-Host ("{0}: SR={1} DE={2} NC={3} VF={4} IF={5} ID={6} EMDASH={7} WE={8}" -f $name, $sr, $de, $nc, $vf, $ii, $id, $em, $we)
}
