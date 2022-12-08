import { useRef, useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import {
  Modal,
  Backdrop,
  Fade,
  Grid,
  Typography,
  Button,
  InputLabel,
  MenuItem,
  FormControl,
  Select,
  TextField,
  FormHelperText,
} from "@material-ui/core";

import PostAddIcon from "@material-ui/icons/PostAdd";
import WatchIcon from "@material-ui/icons/AccessTimeSharp";
import ShopIcon from "@material-ui/icons/Storefront";
import ServiceIcon from "@material-ui/icons/LocalCarWash";
import PriceIcon from "@material-ui/icons/MonetizationOn";

const useStyles = makeStyles((theme) => ({
  modal: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: "rgba(0,0,0,0.7)",
    zIndex: 2000,
  },
  paper: {
    border: "10px solid dodgerblue",
    borderTop: "4px solid dodgerblue",
    outline: "none",
    boxShadow: theme.shadows[5],
    padding: 0,
    flexDirection: "column",
    minWidth: "50em",
    maxWidth: "1000px",
    backgroundColor: "white",
    borderRadius: "0 0 2.5em 2.5em",
  },
  infoHeader: {
    backgroundColor: "dodgerblue",
    justifyContent: "center",
    padding: "0.3em 0 0.6em 0",
  },
  infoBody: {
    backgroundColor: "white",
    justifyContent: "space-between",
    padding: "2em",
  },
  modalTitle: {
    fontFamily: "Orbitron",
    color: "white",
    fontWeight: 700,
  },
  modalDesc: {
    fontFamily: "Orbitron",
    color: "dodgerblue",
    fontWeight: 500,
    textAlign: "center",
    margin: "0.5em 0",
    color: "dodgerblue",
  },
  icon: { fontSize: 70, color: "white", marginRight: "0.3em" },
  btnGrid: {
    borderRadius: "0 0 1.8em 1.8em",
  },
  btn: {
    width: "100%",
    height: "100%",
    fontFamily: "Orbitron",
    fontSize: 30,
    color: "dodgerblue",
    borderTop: "4px dashed dodgerblue",
    transition: "0.15s linear",
    "&:hover": {
      backgroundColor: "dodgerblue",
      color: "white",
    },
  },
  formControl: {
    minWidth: 120,
  },
  selectEmpty: {
    marginTop: theme.spacing(2),
  },
  arrowIcon: {
    fill: "dodgerblue",
  },
  select: {
    color: "dodgerblue",
    fontWeight: 700,
    textAlign: "center",
    fontSize: 20,
    width: "8rem",
    "& .MuiSelect-root": {
      "&:focus": {
        backgroundColor: "transparent",
        color: "dodgerblue",
      },
    },
  },
  menuPaper: {
    height: "14em",
  },
  menuItem: {
    color: "dodgerblue",
    fontFamily: "Orbitron",
    fontWeight: 700,
    textAlign: "center",
  },
  datePicker: {
    "& .MuiInputLabel-root": {
      color: "dodgerblue",
      fontSize: "1rem",
      fontFamily: "Orbitron",
      fontWeight: 700,
      fontSize: 20,
    },
    "& .MuiInputBase-input": {
      color: "dodgerblue",
      fontWeight: 700,
      fontSize: 20,
      "&:focus": {
        color: "dodgerblue",
      },
    },
    "& .Mui-error .MuiInputBase-input": {
      color: "red",
      fontWeight: 700,
      fontSize: 20,
      "&:focus": {
        color: "dodgerblue",
      },
    },
    "& .Mui-error": {
      color: "red",
    },
    "& ::-webkit-calendar-picker-indicator": {
      filter:
        "invert(42%) sepia(21%) saturate(3652%) hue-rotate(186deg) brightness(106%) contrast(101%)",
      fontSize: 26,
      "&:hover": {
        cursor: "pointer",
      },
    },
    "& .MuiFormHelperText-root.Mui-error": {
      color: "red",
      fontFamily: "Orbitron",
      fontSize: 12,
      userSelect: "none",
      textAlign: "right",
    },
    "& .MuiFormHelperText-root": {
      color: "transparent",
      margin: 0,
      marginBottom: "10px",
      fontSize: 12,
      userSelect: "none",
    },
  },
  titleIcon: {
    color: "dodgerblue",
    fontSize: 35,
    marginRight: "3px",
  },
  title: {
    color: "dodgerblue",
    fontFamily: "Orbitron",
    fontWeight: 700,
    fontSize: 20,
  },

  iconTitleContainer: {
    width: "auto",
    padding: "1em",
    borderRadius: "12px",
    alignItems: "center",
  },
  selectErrorText: {
    fontFamily: "Orbitron",
    fontSize: 12,
    margin: 0,
    userSelect: "none",
    textAlign: "right",
  },
}));

export default function NewBookingModal(props) {
  const css = useStyles();
  const [time, setTime] = useState("");
  const dateRef = useRef();

  const [timeError, setTimeError] = useState(false);
  const [dateError, setDateError] = useState(false);

  const closeModalHandle = () => {
    setTimeError(false);
    setDateError(false);
    props.closeModal((prevValues) => {
      return { ...prevValues, bool: false };
    });
  };

  const handleChange = (event) => {
    setTime(event.target.value);
  };

  const hourSelection = [];

  for (let i = 1; i <= 24; i++) {
    hourSelection.push(i);
  }

  return (
    <Modal
      className={css.modal}
      open={props.promptModal.bool}
      closeAfterTransition
      onClose={closeModalHandle}
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
      }}
    >
      <Fade in={props.promptModal.bool}>
        <Grid className={css.paper}>
          <Grid container item alignItems="center" className={css.infoHeader}>
            <PostAddIcon className={css.icon} />
            <Typography variant="h3" className={css.modalTitle}>
              {props.promptModal.title}
            </Typography>
          </Grid>

          <Grid container item className={css.infoBody}>
            <Grid
              container
              item
              style={{
                width: "auto",
              }}
            >
              <Grid container item direction="column" alignItems="flex-start">
                <Grid container item className={css.iconTitleContainer}>
                  <ShopIcon className={css.titleIcon} />
                  <Typography className={css.title}>
                    {props.promptModal.shopName}
                  </Typography>
                </Grid>

                <Grid container item className={css.iconTitleContainer}>
                  <ServiceIcon className={css.titleIcon} />
                  <Typography className={css.title}>
                    {props.promptModal.serviceName}
                  </Typography>
                </Grid>

                <Grid container item className={css.iconTitleContainer}>
                  <PriceIcon className={css.titleIcon} />
                  <Typography className={css.title}>
                    {props.promptModal.servicePrice}
                  </Typography>
                </Grid>
              </Grid>
            </Grid>
            <Grid
              container
              item
              alignItems="flex-end"
              justifyContent="space-around"
              direction="column"
              style={{ width: "auto", marginRight: "1em" }}
            >
              <Grid container item style={{ width: "auto" }}>
                <TextField
                  inputRef={dateRef}
                  variant="outlined"
                  label="Date"
                  type="date"
                  defaultValue={""}
                  className={css.datePicker}
                  error={dateError}
                  helperText={"* Required"}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  onFocus={() => {
                    setDateError(false);
                  }}
                />
              </Grid>
              <Grid container item style={{ width: "auto" }}>
                <FormControl variant="outlined" className={css.formControl}>
                  <InputLabel>
                    <Grid
                      container
                      item
                      style={{ position: "relative", bottom: "3px" }}
                    >
                      <WatchIcon
                        style={{
                          color: `${timeError ? "Red" : "Dodgerblue"}`,
                          marginRight: "2px",
                        }}
                      />
                      <Typography
                        style={{
                          color: `${timeError ? "Red" : "Dodgerblue"}`,
                          fontWeight: 900,
                          fontFamily: "Orbitron",
                          fontSize: 20,
                          bottom: "3px",
                          position: "relative",
                        }}
                      >
                        Time
                      </Typography>
                    </Grid>
                  </InputLabel>
                  <Select
                    value={time}
                    onChange={handleChange}
                    onFocus={() => {
                      setTimeError(false);
                    }}
                    label="TtTime"
                    inputProps={{
                      classes: {
                        icon: css.arrowIcon,
                      },
                    }}
                    MenuProps={{
                      classes: { paper: css.menuPaper },
                      anchorOrigin: {
                        vertical: "bottom",
                        horizontal: "left",
                      },
                      getContentAnchorEl: null,
                    }}
                    className={css.select}
                    error={timeError}
                  >
                    <MenuItem value="" className={css.menuItem}>
                      None
                    </MenuItem>
                    {hourSelection.map((x) => (
                      <MenuItem key={x} value={x} className={css.menuItem}>
                        {x}
                      </MenuItem>
                    ))}
                  </Select>

                  <FormHelperText
                    className={css.selectErrorText}
                    style={{ color: `${timeError ? "red" : "white"}` }}
                  >
                    * Required
                  </FormHelperText>
                </FormControl>
              </Grid>
            </Grid>
          </Grid>
          <Grid container item className={css.btnGrid}>
            <Grid container item>
              <Button
                className={css.btn}
                disableRipple
                onClick={() => {
                  const isValid = dateRef.current.value !== "" && time !== "";

                  if (isValid) {
                    props.createBookingHandle({
                      date: dateRef.current.value,
                      time: time,
                    });
                  } else {
                    setTimeError(() => {
                      return time === "";
                    });

                    setDateError(() => {
                      return dateRef.current.value === "";
                    });
                  }
                }}
                style={{
                  borderRadius: " 0 0 20px 20px",
                  fontWeight: 900,
                }}
              >
                CREATE
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </Fade>
    </Modal>
  );
}
