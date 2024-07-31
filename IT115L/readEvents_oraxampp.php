<?php

include_once('connect_oraxampp.php');
$day_num = $_GET['day_num'];
// Query to select event names from the event table
$query = "SELECT event_name FROM event WHERE event_day = 'Day $day_num'";

// Parse the SQL query
$statement = oci_parse($dbconn, $query);

// Execute the SQL query
$result = oci_execute($statement);

if ($result) 
{
    $eventNames = array();
    while (($row = oci_fetch_assoc($statement)) != false) {
        $eventNames[] = $row['EVENT_NAME'];
    }

    // Check if any event names were fetched
    if (empty($eventNames)) {
        echo json_encode(array("message" => "No data found."));
    } else {
        echo json_encode($eventNames);
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