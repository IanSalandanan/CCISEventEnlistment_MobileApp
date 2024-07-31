<?php

$user = "system";
$pass = "kerby123";
$host = "localhost/XE";
$dbconn = oci_connect($user, $pass, $host);

if (!$dbconn) {echo oci_error(); }
?>

