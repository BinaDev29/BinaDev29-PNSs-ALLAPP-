// PNS-FrontEnd/src/app/styles/theme.ts
import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: { // የቀለም ስብስቦች
    primary: {
      main: '#1976d2', // ለዋና ነገሮች የሚሆን ሰማያዊ ቀለም
    },
    secondary: {
      main: '#dc004e', // ለሁለተኛ ደረጃ ነገሮች የሚሆን ቀይ ቀለም
    },
    background: {
      default: '#f4f6f8', // የዳራ ቀለም (ቀላል ግራጫ)
      paper: '#ffffff', // ለካርዶች እና ሌሎች ኤለመንቶች ነጭ ዳራ
    },
  },
  typography: { // የጽሁፍ ስታይሎች
    fontFamily: 'Roboto, Arial, sans-serif',
    h5: {
      fontWeight: 600,
    },
  },
  components: { // ለተለያዩ ኮምፖነንቶች የራሳችንን ስታይል
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8, // ለbuttons የተጠጋጋ ጠርዝ
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          borderRadius: 8, // ለtext fields የተጠጋጋ ጠርዝ
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundColor: '#282c34', // ለ AppBar ጥቁር ቀለም
        },
      },
    },
  },
});

export default theme;