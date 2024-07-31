
<?php

include_once('connect_oraxampp.php');

$id = $_GET['id'];
$password = $_GET['password'];

// Prepare SQL statement for selecting data
$sql = "SELECT * FROM student WHERE stud_id = $id AND password = '$password'";

// Parse SQL statement
$statement = oci_parse($dbconn, $sql);

// Execute SQL statement
oci_execute($statement);

// Fetch the result
$row = oci_fetch_assoc($statement);

if (!$row) {
    echo "Failed!";
} else {
    echo "OK!";
}

// Clean up resources
oci_free_statement($statement);
oci_close($dbconn);

?>
