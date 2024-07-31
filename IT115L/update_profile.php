<?php
// Include the database connection file
include_once('connect_oraxampp.php');

// Get data from the request body (assumes JSON format)
$data = json_decode(file_get_contents('php://input'), true);

// Extract data
$studId = $data['Stud_id'] ?? null;
$password = $data['Password'] ?? null;
$program = $data['Program'] ?? null;
$year_lvl = $data['Year_lvl'] ?? null;

if ($studId && $password && $program && $year_lvl) {
    // Update query
    $query = "UPDATE Student
              SET Password = :password,
                  Program = :program,
                  Year_lvl = :year_lvl
              WHERE Stud_id = :studId";

    // Parse the SQL query
    $statement = oci_parse($dbconn, $query);

    // Bind parameters
    oci_bind_by_name($statement, ':password', $password);
    oci_bind_by_name($statement, ':program', $program);
    oci_bind_by_name($statement, ':year_lvl', $year_lvl);
    oci_bind_by_name($statement, ':studId', $studId);

    // Execute the SQL query
    $result = oci_execute($statement);

    if ($result) {
        echo json_encode(array("message" => "Profile updated successfully."));
    } else {
        $error = oci_error($statement);
        echo json_encode(array("message" => "Failed to update profile.", "error" => $error['message']));
    }

    // Clean up resources
    oci_free_statement($statement);
} else {
    echo json_encode(array("message" => "Invalid input data."));
}

oci_close($dbconn);
?>
