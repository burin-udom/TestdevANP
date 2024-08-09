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
    $user_username = $_POST['username'];
    $user_password = $_POST['password'];

    // Prepare and bind for checking login credentials
    $stmt = $conn->prepare("SELECT id, password FROM users WHERE username = ?");
    $stmt->bind_param("s", $user_username);

    // Execute the statement
    if ($stmt->execute()) {
        $stmt->store_result();

        if ($stmt->num_rows > 0) {
            $stmt->bind_result($user_id, $hashed_password);
            $stmt->fetch();

            //$hashed_password = password_hash($hashed_password, PASSWORD_DEFAULT);
            // Debugging information
            error_log("DEBUG: Provided password: $user_password");
            error_log("DEBUG: Hashed password from DB: $hashed_password");
            //echo json_encode(['success' => false, 'message' => 'Authorizing Username']);

            if (password_verify($user_password, $hashed_password)) {
                // Password is correct
                // Get current time
                $login_time = date('Y-m-d H:i:s');

                // Insert login history with current login time
                $stmt_login_history = $conn->prepare("INSERT INTO login_history (user_id, login_time) VALUES (?, ?)");
                $stmt_login_history->bind_param("is", $user_id, $login_time);
                $stmt_login_history->execute();
                $stmt_login_history->close();

                // Retrieve user data
                $stmt_user_data = $conn->prepare("SELECT diamonds, hearts FROM user_data WHERE user_id = ?");
                $stmt_user_data->bind_param("i", $user_id);

                if ($stmt_user_data->execute()) {
                    $stmt_user_data->bind_result($diamonds, $hearts);
                    $stmt_user_data->fetch();
                    echo json_encode([
                        'success' => true, 
                        'message' => 'Login successful!', 
                        'user_id' => $user_id, 
                        'diamonds' => $diamonds, 
                        'hearts' => $hearts
                    ]);
                    $stmt_user_data->close();
                } else {
                    echo json_encode(['success' => false, 'message' => 'Error retrieving user data']);
                }
            } else {
                // Password is incorrect
                echo json_encode(['success' => false, 'message' => "Invalid password"]);
            }
        } else {
            // Username not found
            echo json_encode(['success' => false, 'message' => 'Invalid username']);
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