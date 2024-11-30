const fs = require('fs');
const path = './src/environments';

if(!process.env.production) {
  console.log('Production environment variable not set!');
  process.exit(1);
}

if (!fs.existsSync(path)) {
  fs.mkdirSync(path, { recursive: true }); // Create the folder if it doesn't exist
  console.log('Environments folder created!');
}

  // Fetch environment variables from process.env
  const environment = `
    export const environment = {
        production: ${process.env.production},
        apiEndpoint: '${process.env.apiEndpoint}'
    };
    `;

  // Write the file to the environments folder
  fs.writeFileSync(`${path}/environment.ts`, environment);
  console.log(environment);
  console.log('Production environment file created successfully!');
