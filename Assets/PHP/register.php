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

// Check connection
if ($conn->connect_error) {
    die(json_encode(['success' => false, 'message' => 'Connection failed: ' . $conn->connect_error]));
}

// Check if the request method is POST
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Retrieve the POST data
    $user_username = $_POST['username'];
    $user_password = $_POST['password'];

    // Check if the username already exists
    $stmt_check = $conn->prepare("SELECT id FROM users WHERE username = ?");
    $stmt_check->bind_param("s", $user_username);
    $stmt_check->execute();
    $stmt_check->store_result();

    if ($stmt_check->num_rows > 0) {
        // Username already exists
        echo json_encode(['success' => false, 'message' => 'Username already exists']);
        $stmt_check->close();
    } else {
        // Username does not exist, proceed with registration
        $stmt_check->close();

        // Hash the password before storing it
        $hashed_password = password_hash($user_password, PASSWORD_DEFAULT);

        // Prepare and bind for inserting into users table
        $stmt = $conn->prepare("INSERT INTO users (username, password) VALUES (?, ?)");
        $stmt->bind_param("ss", $user_username, $hashed_password);

        // Execute the statement and get the last inserted ID
        if ($stmt->execute()) {
            $user_id = $stmt->insert_id;

            // Prepare and bind for inserting into user_data table
            $stmt_user_data = $conn->prepare("INSERT INTO user_data (user_id, diamonds, hearts) VALUES (?, 0, 10)");
            $stmt_user_data->bind_param("i", $user_id);

            if ($stmt_user_data->execute()) {
                echo json_encode(['success' => true, 'message' => 'Registration successful!']);
            } else {
                echo json_encode(['success' => false, 'message' => 'Error: ' . $stmt_user_data->error]);
            }

            $stmt_user_data->close();
        } else {
            echo json_encode(['success' => false, 'message' => 'Error: ' . $stmt->error]);
        }

        // Close statement and connection
        $stmt->close();
        $conn->close();
    }
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid request method']);
}
?>