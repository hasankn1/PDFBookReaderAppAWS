# 📚 Bookshelf eReader App

## 🌟 Overview

This project is a Bookshelf eReader Application built as part of the API Engineering & Cloud Computing COMP306 Lab 2. 
The app mimics the functionality of popular online eReaders by integrating AWS DynamoDB for user and book management and AWS S3 for book content storage. 
Users can log in, sign up, view their bookshelf, read books, and save their progress with automatic bookmarking.

## 🛠️ Features

    User Login:
        Allows users to securely log in and access their bookshelf.
    
    User Sign up:
        Allows users to signup to start reading.

    Bookshelf Management:
        Lists all books on the user’s bookshelf, sorted by the most recently read.

    Book Reading and Bookmarking:
        Enables users to read books stored in AWS S3 using the Syncfusion PDF Viewer.
        Automatically bookmarks the last-read page when the user closes the reader or clicks the "Bookmark" button.

    AWS Integration:
        DynamoDB: Stores user data, books metadata, and reading activity.
        S3: Stores the actual book files securely.

## 📋 Prerequisites

AWS DynamoDB Table: Populate sample data using this [File](https://github.com/hasankn1/PDFBookReaderAppAWS/blob/master/Bookshelf%20Table%20sample%20data%20in%20DynamoDB.txt).
![Screenshot 2024-12-04 153955](https://github.com/user-attachments/assets/a0f7ecff-f89d-47c9-92c8-c3e44f8fba47)
![Screenshot 2024-12-04 153234](https://github.com/user-attachments/assets/3f484e1f-d3c6-4dfc-91b5-68f6b9807c81)
![Screenshot 2024-12-04 153226](https://github.com/user-attachments/assets/e6f4a7b6-f55f-491f-9408-5002d00673bc)
![Screenshot 2024-12-04 153153](https://github.com/user-attachments/assets/c4680a30-2ea7-42b2-8796-57bff4c518e3)
![Screenshot 2024-12-04 153130](https://github.com/user-attachments/assets/bb213107-94cd-4a6a-b219-0a0399bee39d)
AWS S3 Buckets with books:
![Screenshot 2024-12-04 154049](https://github.com/user-attachments/assets/c26b9adc-2982-4c8e-af74-1230aaf0405f)
![Screenshot 2024-12-04 154026](https://github.com/user-attachments/assets/6ca96cf2-4837-4fdd-8833-fb4667c33a0c)
AWS IAM Credentials: Ensure valid access to AWS services.

## 🧰 Technical Stack

    Frameworks & Tools:
        WPF (.NET 8)
        Syncfusion PDF Viewer

    Cloud Services:
        AWS DynamoDB: NoSQL database for user and book data.
        AWS S3: Secure cloud storage for book PDFs.

    Languages:
        C#, XAML

## 🖼️ Screenshots

Home Page:
![Screenshot 2024-12-04 151350](https://github.com/user-attachments/assets/78685127-a0f5-417f-b5fd-796e2fda420a)

Sign Up:
![Screenshot 2024-12-04 152820](https://github.com/user-attachments/assets/f9765a0e-a79e-4208-bba8-e3279b292cf4)
![Screenshot 2024-12-04 152832](https://github.com/user-attachments/assets/286c240e-ba2f-4a3b-9bfa-908863fcb106)

Sign In:
![Screenshot 2024-12-04 152840](https://github.com/user-attachments/assets/b5d05403-0c83-4efb-972c-eb8674d88b0d)

Bookshelf Window:
![Screenshot 2024-12-04 152846](https://github.com/user-attachments/assets/073f6790-21c6-44e5-b6f0-5397721b7211)

Reading an eBook:
![Screenshot 2024-12-04 152904](https://github.com/user-attachments/assets/df770090-b733-404d-8ae4-9aa96766dfaa)
![Screenshot 2024-12-04 152920](https://github.com/user-attachments/assets/33a7deb9-9719-417b-a3bc-3c422df54518)

Bookmarking a page:
![Screenshot 2024-12-04 152936](https://github.com/user-attachments/assets/67fbf461-3b5d-4535-ae35-b737b6e37896)

Opening another Book:
![Screenshot 2024-12-04 153009](https://github.com/user-attachments/assets/771d3b2f-9cb9-40de-a2bd-acdc6581d599)
![Screenshot 2024-12-04 153023](https://github.com/user-attachments/assets/43bff957-492e-4b01-92e7-478dda29f945)

Auto Bookmarking Functionality:
![Screenshot 2024-12-04 153033](https://github.com/user-attachments/assets/abcbf703-3074-47ed-80f2-90b846a6492a)

## 🎯 Project Goals

    AWS DynamoDB Setup:
        Create a Bookshelf table to store user and book data.
        Populate the table with sample data for at least 3 users, each having 2 books.

    WPF Application Development:
        Implement user login functionality.
        List books in the bookshelf with the most recently accessed book at the top.
        Load book content securely from AWS S3 using code-behind logic.

    Bookmarking Functionality:
        Save the current page number and timestamp to DynamoDB when users bookmark or close the reader.

    Code Quality:
        Maintain high-quality, efficient, and readable code.

## 📜 License

    This project is licensed under the MIT License.
