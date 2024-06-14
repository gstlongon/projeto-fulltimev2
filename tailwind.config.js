  // tailwind.config.js
  module.exports = {
    content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
     darkMode: 'media', // or 'media' or 'class'
     theme: {
       extend: {},
     },
     variants: {
       extend: {},
     },
     plugins: [],
     theme: {
      fontSize: {
        sm: ['14px', '20px'],
        base: ['16px', '24px'],
        base2: ['18px', '26px'],
        lg: ['20px', '28px'],
        xl: ['30px', '38px'],
      }
    }
   }