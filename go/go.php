<?php
    $href = 'articles/Welcome.html';
    $guid = strtolower($_REQUEST['guid']);

    // Check for valid GUID
    if(preg_match('/^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$/i', $guid) == 1) 
    {
        // Find href in XrefMap
        $xrefmap = require('./xrefmap.php');
        if(array_key_exists($guid, $xrefmap))
        {
            $href = $xrefmap[$guid];
        }
    }

    header("Location: ../$href");
    exit;
?>
