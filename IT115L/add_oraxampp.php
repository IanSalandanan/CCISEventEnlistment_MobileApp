<?php

include_once('connect_oraxampp.php');

$get_id = $_GET['id'];
$get_password = $_GET['passWord']; 
$get_fName = $_GET['fName'];
$get_lName = $_GET['lName'];
$get_yrLvl = $_GET['yearLvl'];
$get_program = $_GET['program'];
$get_house = $_GET['house'];

$sql = "INSERT INTO student VALUES($get_id, '$get_password', '$get_fName', '$get_lName', '$get_yrLvl', '$get_program', '$get_house')";
$statement = oci_parse($dbconn, $sql);

$result = oci_execute($statement);

if ($result) {
  echo "Data inserted successfully.";
} else {
  echo "Error inserting data: " . oci_error($statement)['message'];
}

oci_free_statement($statement);
oci_close($dbconn);

?>