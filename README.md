<div>
  <img src="https://user-images.githubusercontent.com/62003240/129524820-b74fa54f-babe-4b1f-9f83-0baa1aebb923.jpg" width="280"   />
  <img src="https://img.shields.io/badge/Target%20Framework-.NET%205.0-blue" hspace="35" /> 
<div/>

<br/>
  
A project management desktop application that keeps track of a project's tasks and organizes them. (Created using WPF and LocalDB)

<hr/>
  
<br/>

# Purpose
  - To know when the project can be called "DONE" 
    (by implementing the "Important functionalities" (Finish Line Tasks) first before the "Extra Features")
  - We will only implement the "Extra Features", when all the "Important Functionalities" (Finish Line Tasks) are finished AND there is still time left before the deadline
  - To focus on the priorities rather than the "Extra Features" (e.g. animations, and themes)
  - To have an organized plan when creating a project
  
<br/>
  
# Features
  - An option to manage your personal and paid projects
  - Add tasks (with categories, and priorities) to your project 
  - A filter functionality for a project's tasks, for better task organization
  - Add subtasks to a task
  - Add Notes to a project
  - Bonus: store project ideas (that can be potentially created in the future 😉)

<br/>

# How To Download
  <a href="https://github.com/paraJdox1/Paraject/releases">Go to the project's "Releases" section, and select the .exe, .zip, or .7z file to download this app.</a>

<br/>

# How to Use
  
### Login / Create Account
  
- Enter your login credentials if you already have an account.
  
  <img src="https://user-images.githubusercontent.com/62003240/129846021-a30f3758-68f8-4726-8b71-0e958c8ac4a2.png" width="500"   />

  <br/>
  <br/>
  
- Create your user account if you don't have an account.
  
  <img src="https://user-images.githubusercontent.com/62003240/129846244-c4bd798c-aa87-47af-9f70-8253db18bff0.png" width="500"   />

  <br/>
  <br/>
  <br/>
  
### Projects
  
- Manage your project's in the ProjectsView

  <img src="https://user-images.githubusercontent.com/62003240/129846778-98e6910b-3582-4fe6-89f3-c525bca9fbc5.png" width="800"   />
 
  <br/>
  <br/>
  
- View your completed project's in the Completed tab

  <img src="https://user-images.githubusercontent.com/62003240/129847000-13574e50-b76a-4dbc-be08-a17fb1b628ec.png" width="800"   />  
 
  <br/>
  <br/>

- You can modify a project by selecting it (the project's content will be displayed), then select Project Details tab
  
  <img src="https://user-images.githubusercontent.com/62003240/129847259-40e8eec6-38ed-4416-acd1-80580d8c3bff.png" width="800"   />  
  
  <br/>
  <br/>
  <br/>

### Project's Tasks

- Add and manage your Finish Line Tasks (the IMPORTANT tasks) and Extra Feature tasks in the Project's Content (this is displayed if you select a project)

  <img src="https://user-images.githubusercontent.com/62003240/129848072-3448eace-29b9-4417-9467-ca727579a82a.png" width="800"   />  
 
  <br/>
  <br/>
  
- Use the ComboBoxes to enable the filtering of tasks
  
  <img src="https://user-images.githubusercontent.com/62003240/129849188-9753f6f0-5908-4904-b4ad-4c295731990e.png" width="800"   />  
 
  <br/>
  <br/>
  
- View your Completed Tasks
  
  <img src="https://user-images.githubusercontent.com/62003240/129849394-e2e6c6cc-98a7-4c9e-9bf8-74652fe9cf61.png" width="800"   />  
 
  <br/>
  <br/>
  
- You can modify a task by selecting it (the task's content will be displayed), then select Task Details tab

  <img src="https://user-images.githubusercontent.com/62003240/129849576-5306ffcb-d973-4239-b075-98672cd1157b.png" width="800"   />  
  
  <br/>
  <br/>
  <br/>  

### Task's Subtasks
  
- Add and manage a task's subtasks in the Task's Content
  
  <img src="https://user-images.githubusercontent.com/62003240/129849930-ccef240f-1e93-4db3-b3fd-41475ed34e0e.png" width="800"   />  
   
  <br/>
  <br/>
  
- View your Completed Subtasks
 
  <img src="https://user-images.githubusercontent.com/62003240/129850086-aac9f7f0-254f-4998-b976-f3ffd2e1f672.png" width="800"   />   
   
  <br/>
  <br/>
  
- You can modify a subtask by selecting it (the subtask details modal dialog will be shown)

  <img src="https://user-images.githubusercontent.com/62003240/129850493-bc46b01a-f08b-4dff-aa36-41f8e03317c5.png" width="800"   />  
    
  <br/>
  <br/>
  <br/>  

### BONUS: Project Ideas
  
- A user interface to manage your project ideas 😉

  <img src="https://user-images.githubusercontent.com/62003240/129850995-85e22b93-27a1-48e5-a915-54ac4839d63c.png" width="800"   />  
  
<br/>
  
# Things to Note
  - To generate the project's final output (.exe)
    - Check the Configuration (Debug or Release) and Platform (x86 (32 bit) or x64) before `Building`
    - Delete the Project's bin folder if it can't copy the .mdf or .ldf files (this project uses a service-based database)
    1. Build `Paraject.Installer` (the installer builds the project (Paraject) in its before build (located in Paraject.Installer.wixproj))
    1. Build `Paraject.Bootstrapper`
    1. The [Bootstrapper].exe is the one that packages the app's installer, and the app's prerequisite (this will serve as the final output)
  
  - If you have SqlLocalDB (2016) already installed in your machine, it will also be uninstalled if you uninstall this app. (Check your `Control Panel` to view changes)
  - This app installs SqlLocalDB.msi (2016). When the app is uninstalled your SqlLocalDB (2016) is uninstalled too.
