# CompuMat Control Panel

#### Disclaimer
This application is a proof-of-concept. It is not in a deployable state, where it is ready to run on any machine out-of-the-box. 
Issues may occur, to the point where the application is faulty or unable to run 
if versions, paths or data is not similar or identical to that of the environment it was developed in.
It does not include any private, sensitive or proprietary data, functionality or software, that is not developed entirely by the two contributors of the project.

#### The following is required before the application can be run:
* RabbitMQ is installed and a local RabbitMQ-server is running on the machine
* Visual Studio 2022 is installed
* Visual Studio Code is installed
* Node and Node Package Manager is installed
* A working browser (Chrome or FireFox is recommended)
* A local version of Microsoft SQL Server Management Studio 18 is installed and running
  * The local database should have the following two tables:
  tablename: Compumat
  
| Id |  StationNo  | Name             | Latitude            | Longitude           | Type | Status  |
| -- | ----------- | ---------------- | ------------------- | ------------------- |----- | ------- |
| 2  |     S90     | Gate01           | 55,314044925546511  | 10,787099052273176  | 2    | ok      |
| 3  |     S97     | VendingMachine01 | 55,315044925546509  | 10,787099052273176  | 1    | error   |
| 4  |     S91     | Gate02           | 55,313044925546507  | 10,787099052273176  | 2    | offline |
  
  tablename: Site
  
| Id |  CampSiteName  | Latitude        | Longitude           |
| -- | ----------- | ------------------ | ------------------- | 
| 1  | test        | 55,314044925546511 | 10,787099052273176  |

#### Initial Setup:
* Download and unzip the repository
* Open "<local-path>/afgangsprojekt/frontend" in VS-Code
  * In a terminal inside vs-code, run the following commands:
    * > npm install
    * > ng build
* Open "<local-path>/afgangsprojekt/API/API.sln" in Visual Studio 2022
  * Run the solution
* Back in the VS-code terminal, run:
  * > ng server --open
    * If your browser does not automatically open the application, navigate to URL: "localhost:4200"

#### To test the application with device-events:
* Follow the steps in the initial setup.
* Open "<local-path>/afgangsprojekt/API/CompumatServer.sln" in a new Visual Studio 2022 window.
* Run the solution.
  * Ensure that the API.sln is also running and that the frontend is served and accessible
* Check the output-terminal from the API-project and the web-application on "localhost:4200" to see the events being handled and displayed.
