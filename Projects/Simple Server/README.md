# PURPOSE

This is an example server.

# SETUP

Install node, npm.  After you clone the repository, run `npm install` which will update all your dependencies.

# TO RUN THE SERVER FOR DEVELOPMENT

Run `npm run watch` which will auto compile and reload the server when you save a change in a source file. You can test the output in a browser at `http://localhost:3000/

# PUSHING TO HEROKU

Build using run watch before committing.

Push the project to our heroku server using `git push heroku master`. Then, make sure it's running at least one instance using `heroku ps:scale web=1`, and you can type `heroku open` to automatically open the project.

When pushed to heroku, will be at 'https://wordlist-builder-prototype.herokuapp.com/check?word=somewordtotest'

