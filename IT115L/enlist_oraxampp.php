<?php

include_once('connect_oraxampp.php');

$get_id = $_GET['stud_id']; 
$get_eventName = $_GET['event_name'];
$get_type = $_GET['attendant_type'];

// Query to check the current status of the attendance
$sql1 = "SELECT attendant_type FROM Attendance WHERE stud_id = :id AND event_name = :event_name";
$statement1 = oci_parse($dbconn, $sql1);
oci_bind_by_name($statement1, ':id', $get_id);
oci_bind_by_name($statement1, ':event_name', $get_eventName);
oci_execute($statement1);

$attendant_type = null;
oci_fetch($statement1);
$attendant_type = oci_result($statement1, 'ATTENDANT_TYPE'); //so kinuha ung data ng nakuhang attendance type?

if ($attendant_type === false) //STUDY THIS LINE PLS
{
    // No existing record, insert a new one
    $sql2 = "INSERT INTO Attendance (stud_id, attendant_type, event_name) VALUES (:id, :type, :event_name)";
    $statement2 = oci_parse($dbconn, $sql2);
    oci_bind_by_name($statement2, ':id', $get_id);
    oci_bind_by_name($statement2, ':type', $get_type);
    oci_bind_by_name($statement2, ':event_name', $get_eventName);
    $final_result = oci_execute($statement2);
} elseif ($attendant_type == 'Withdrawn') 
{
    // Existing record with 'Withdrawn', update it
    $sql3 = "UPDATE Attendance SET attendant_type = :type WHERE stud_id = :id AND event_name = :event_name";
    $statement3 = oci_parse($dbconn, $sql3);
    oci_bind_by_name($statement3, ':id', $get_id);
    oci_bind_by_name($statement3, ':type', $get_type);
    oci_bind_by_name($statement3, ':event_name', $get_eventName);
    $final_result = oci_execute($statement3);
} else 
{
    echo "Error: Attendant type is not 'Withdrawn' and record exists.";
    exit;
}

if ($final_result) {
    echo "Successful!";
} else {
    $e = oci_error($statement1);
    echo "Error inserting/updating data: " . $e['message'];
}

oci_free_statement($statement1);
if (isset($statement2)) oci_free_statement($statement2);
if (isset($statement3)) oci_free_statement($statement3);
oci_close($dbconn);

?>
