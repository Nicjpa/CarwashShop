import { createTheme } from "@material-ui/core";
import lamboBCG from "./images/mustang.jpg";
import { useLocation } from "react-router-dom";

const arcWhite = "#ffffff";
const arcBlue = "#1e90ff";

const Theme = createTheme({
  palette: {
    background: {
      default: `${arcWhite}`,
    },
    primary: {
      main: `${arcWhite}`,
    },
    secondary: {
      main: `${arcBlue}`,
    },
    text: {
      disabled: "whitesmoke",
    },
  },
  typography: {
    tab: {
      fontFamily: "Raleway",
      textTransform: "None",
      fontWeight: 700,
      fontSize: "1rem",
    },
    estimate: {
      fontFamily: "Pacifico",
      fontSize: "1rem",
      textTransform: "none",
      color: "white",
    },
  },
  overrides: {
    MuiInputLabel: {
      root: {
        color: "white",
        fontSize: "1rem",
        fontFamily: "Orbitron",
        fontWeight: 500,
      },
    },
    MuiInputBase: {
      root: {
        color: "white",
        fontFamily: "Orbitron",
        fontWeight: 600,
        "&:focus": {
          borderColor: "dodgerblue",
          color: "white",
        },
      },
      input: {
        color: "dodgerblue",
        "&.Mui-disabled": {
          color: "white",
        },
        "&:focus": {
          color: "white",
        },
      },
    },
    MuiOutlinedInput: {
      root: {
        "& $notchedOutline": {
          border: "2px dodgerblue solid",
          borderRadius: "12px",
        },
        "&:hover $notchedOutline": {
          borderColor: "dodgerblue",
        },
        "&:focus $notchedOutline": {
          borderColor: "dodgerblue",
        },
        "&:focus ": {
          borderColor: "dodgerblue",
        },
        "&.Mui-focused .Mui-focused": {
          borderColor: "dodgerblue",
        },
        "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
          borderColor: "dodgerblue",
        },
        "&.Mui-disabled $notchedOutline": {
          borderColor: "white",
        },
      },
    },
    MuiSelect: {
      select: {
        "&:focus": {
          backgroundColor: "transparent",
          color: "white",
        },
      },
    },
    MuiFormLabel: {
      root: { "&.Mui-focused": { color: "dodgerblue" } },
    },
    MuiSlider: {
      rail: { backgroundColor: "skyblue", height: "9px" },
      track: {
        backgroundColor: "dodgerblue",
        height: "9px",
      },
      thumb: {
        width: "18px",
        height: "18px",
        "&.Mui-disabled": {
          width: "18px",
          height: "18px",
          marginTop: "-5px",
          marginLeft: "-6px",
          backgroundColor: "white",
        },
      },
    },
    MuiCssBaseline: {
      "@global": {
        body: {
          background: `linear-gradient(360deg, rgba(67, 206, 162, 0.6)  15%, rgba(24, 90, 157, 0.6) 60%), url(${lamboBCG}) no-repeat center bottom / cover`,
          backgroundRepeat: "no-repeat",
          backgroundAttachment: "fixed",
        },
      },
    },
  },
});

export default Theme;
