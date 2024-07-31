<?php

include_once('connect_oraxampp.php');

$id = $_GET['id'];

// Prepare SQL statement for selecting data
$sql = "SELECT * FROM student WHERE stud_id = $id";

// Parse SQL statement
$statement = oci_parse($dbconn, $sql);

// Execute SQL statement
oci_execute($statement);

// Fetch the result
$row = oci_fetch_assoc($statement);

if (!$row) {
    echo "OK!";
} else {
    echo "Failed!";
}

// Clean up resources
oci_free_statement($statement);
oci_close($dbconn);

?>
