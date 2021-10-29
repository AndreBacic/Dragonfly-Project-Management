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
        extend: {
            colors: {
                blueGray: colors.blueGray,
                coolGray: colors.coolGray,
                trueGray: colors.trueGray,
                warmGrey: colors.warmGray,
                amber: colors.amber,
                lime: colors.lime,
                emerald: colors.emerald,
                teal: colors.teal,
                cyan: colors.cyan,
                sky: colors.sky,
                lightBlue: colors.lightBlue,
                violet: colors.violet,
                fuchsia: colors.fuchsia,
                rose: colors.fuchsia
            }
        },
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('tailwindcss-textshadow')
    ]
}
