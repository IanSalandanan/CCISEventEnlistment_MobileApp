<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Methods: GET');
header('Access-Control-Allow-Headers: Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');

include_once('connect_oraxampp.php');

$id = $_GET['stud_id'];
$event_name = $_GET['event_name'];

// Query to check current attendant_type
$query_check_type = "SELECT attendant_type FROM Attendance WHERE stud_id = $id AND event_name = '$event_name'";
$statement_check_type = oci_parse($dbconn, $query_check_type);

// Execute query to fetch current attendant_type
$result_check_type = oci_execute($statement_check_type);

if ($result_check_type === false) {
    echo json_encode(array("message" => "Failed to fetch existing data"));
} else {
    // Fetch the attendant_type (assuming there should be only one row)
    $row = oci_fetch_assoc($statement_check_type);
    $attendant_type = $row['ATTENDANT_TYPE'] ?? null;

    if ($attendant_type === null) {
        // No existing record
        echo json_encode(array("message" => "No existing record for the specified stud_id and event_name"));
    } elseif ($attendant_type === 'Withdrawn') {
        // Already withdrawn
        echo json_encode(array("message" => "Already withdrawn."));
    } else {
        // Proceed with updating the record
        // Prepare SQL statement for updating data
        $update_query = "UPDATE Attendance SET attendant_type = 'Withdrawn' WHERE stud_id = $id and event_name = '$event_name'";

        // Parse SQL statement
        $statement_update = oci_parse($dbconn, $update_query);

        // Execute SQL statement
        $result_update = oci_execute($statement_update);

        if ($result_update === false) {
            echo json_encode(array("message" => "Failed to update data"));
        } else {
            echo json_encode(array("message" => "Withdrawn."));
        }

        // Clean up update statement
        oci_free_statement($statement_update);
    }
}

// Clean up resources
oci_free_statement($statement_check_type);
oci_close($dbconn);
?>
