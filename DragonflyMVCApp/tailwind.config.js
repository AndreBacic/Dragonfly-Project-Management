const colors = require("tailwindcss/colors")

module.exports = {
    mode: 'jit',
    purge: [
        './**/*.{html,cshtml,razor}',
        './**/**/*.{html,cshtml,razor}',
    ],
    darkMode: 'media', // false or 'media' or 'class'
    theme: {
        extend: {
            borderRadius: {
                '4xl': '2rem',
                '5xl': '2.5rem',
                '6xl': '3rem',
                '7xl': '3.5rem',
                '8xl': '4rem',
            },
            colors: {
                slate: colors.blueGray,
                gray: colors.gray,
                neutral: colors.trueGray,
                stone: colors.warmGray,
                orange: colors.orange,
                amber: colors.amber,
                lime: colors.lime,
                emerald: colors.emerald,
                teal: colors.teal,
                cyan: colors.cyan,
                sky: colors.sky,
                violet: colors.violet,
                fuchsia: colors.fuchsia,
                rose: colors.rose
            }
        },
    },
    variants: {
        extend: {
            borderRadius: ['hover', 'focus'],
        },
    },
    plugins: [
        require('tailwindcss-textshadow')
    ]
}
