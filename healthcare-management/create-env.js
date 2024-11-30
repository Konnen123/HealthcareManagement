const fs = require('fs');
const path = './src/environments';

if (!fs.existsSync(path)) {
  fs.mkdirSync(path, { recursive: true }); // Create the folder if it doesn't exist
  console.log('Environments folder created!');
}

  // Fetch environment variables from process.env
  const environment = `
    export const environment = {
        production: true,
        apiEndpoint: '${process.env.apiEndpoint}'
    };
    `;

  // Write the file to the environments folder
  fs.writeFileSync(`${path}/environment.ts`, environment);
  console.log('Production environment file created successfully!');
