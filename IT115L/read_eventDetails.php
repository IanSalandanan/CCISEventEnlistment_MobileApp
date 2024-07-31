<?php

include_once('connect_oraxampp.php');

$getEvent_name = $_GET['event_name'];

// Query to select event names from the event table
$query = "SELECT * FROM event WHERE event_name = '$getEvent_name'";

// Parse the SQL query
$statement = oci_parse($dbconn, $query);

// Execute the SQL query
$result = oci_execute($statement);

if ($result) {
    $eventDetails = array();
    while (($row = oci_fetch_assoc($statement)) != false) {
        $eventDetails[] = $row;
    }

    // Check if any event names were fetched
    if (empty($eventDetails)) {
        echo json_encode(array("message" => "No data found."));
    } else {
        echo json_encode($eventDetails);
    }
    
} else {
    // Handle error
    $error = oci_error($statement);
    echo json_encode(array("message" => "Failed to execute query.", "error" => $error['message']));
}

// Clean up resources
oci_free_statement($statement);
oci_close($dbconn);

?>
