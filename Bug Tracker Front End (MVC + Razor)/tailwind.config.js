const colors = require("tailwindcss/colors")

module.exports = {
    mode: 'jit',
    purge: [
        '/wwwroot/*.html',
        '/Views/**/*.cshtml',
        '/Views/*.cshtml'
    ],
    darkMode: 'media', // false or 'media' or 'class'
    theme: {
        extend: {},
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('tailwindcss-textshadow')
    ]
}
