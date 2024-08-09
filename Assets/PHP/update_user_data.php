<?php
header('Content-Type: application/json');

// Database connection parameters
$servername = "103.91.190.179";
$username = "testdev02";
$password = "!wAw-1!fqZWGT_vXR{o({&'&#8GP%tU+";
$dbname = "testdev02";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die(json_encode(['success' => false, 'message' => 'Connection failed: ' . $conn->connect_error]));
}

// Check if the request method is POST
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Retrieve the POST data
    $user_id = $_POST['user_id'];
    $diamonds = $_POST['diamonds'];
    $hearts = $_POST['hearts'];

    // Prepare and bind for updating user data
    $stmt = $conn->prepare("UPDATE user_data SET diamonds = ?, hearts = ? WHERE user_id = ?");
    $stmt->bind_param("iii", $diamonds, $hearts, $user_id);

    // Execute the statement
    if ($stmt->execute()) {
        // Check if any row was actually updated
        if ($stmt->affected_rows > 0) {
            echo json_encode(['success' => true, 'message' => 'User data updated successfully!', 'diamonds' => $diamonds, 'hearts' => $hearts]);
        } else {
            echo json_encode(['success' => false, 'message' => 'No rows updated. User ID might be incorrect.']);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Error: ' . $stmt->error]);
    }

    // Close statement and connection
    $stmt->close();
    $conn->close();
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid request method']);
}
?>
