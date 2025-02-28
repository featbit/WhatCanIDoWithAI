We have created files below:

- index.html
- components/leftbar.js
- components/topbar.js
- components/main.js
- components/footer.js

Now, let's create routing for the project. We will use the following routes:

If click on the left bar menu, it will show the content in the main content section. Then:

- home: show the home content // create a file /components/pages/home.js
- docter management: show the docter management content // create a file /components/pages/docter.js
- department management: show the department management content // create a file /components/pages/department.js
- patient management: show the patient management content // create a file /components/pages/patient.js
- appointment management: show the appointment management content // create a file /components/pages/appointment.js
- report: show the report content // create a file /components/pages/report.js

Create the files if they don't exist.

In the mean while of showing the content:
- the top bar menu should show the brand name and the page title.
- the left bar menu should highlight the current page.

each component file should contains only minimal code, means:

- function component to be called from the main.js
- the title of the page in the component file
- no need to fill the detail of the content, just fill with dummy text

Please modify the files as needed.