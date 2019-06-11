cd src
newman run PaymentsAPI_collection.json -e PaymentsAPI_environment.json --reporters cli,html --reporter-html-export report.html