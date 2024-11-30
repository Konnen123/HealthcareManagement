const fs = require('fs');

// Check if the environment is set to production
if (process.env.NODE_ENV === 'production')
{
  // Fetch environment variables from process.env
  const environment = `
    export const environment = {
        production: true,
        apiEndpoint: '${process.env.API_URL}'
    };
    `;

  // Write the file to the environments folder
  fs.writeFileSync('./src/environments/environment.ts', environment);
  console.log('Production environment file created successfully!');
}
