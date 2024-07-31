<?php
header('Content-Type: application/json');
include_once('connect_oraxampp.php');

$stud_id = $_GET['stud_id'] ?? null;

if ($stud_id) {
    $query = "SELECT Stud_id, Password, First_name, Last_name, Year_lvl, Program, House_name FROM Student WHERE Stud_id = :stud_id";
    $statement = oci_parse($dbconn, $query);
    oci_bind_by_name($statement, ':stud_id', $stud_id);

    $result = oci_execute($statement);

    if ($result) {
        $profile = oci_fetch_assoc($statement);
        if ($profile) {
            echo json_encode($profile);
        } else {
            echo json_encode(array("message" => "No data found."));
        }
    } else {
        $error = oci_error($statement);
        echo json_encode(array("message" => "Failed to execute query.", "error" => $error['message']));
    }

    oci_free_statement($statement);
} else {
    echo json_encode(array("message" => "No student ID provided."));
}

oci_close($dbconn);
?>
