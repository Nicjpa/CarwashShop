import { makeStyles } from "@material-ui/core/styles";
import { useState, useRef } from "react";

import { Grid, TextField, Slider, Typography, Button } from "@material-ui/core";
import ShopIcon from "@material-ui/icons/Storefront";
import ServiceIcon from "@material-ui/icons/LocalCarWash";
import IdIcon from "@material-ui/icons/FormatListNumberedSharp";
import WatchIcon from "@material-ui/icons/AccessTimeSharp";
import RoomIcon from "@material-ui/icons/Room";
import FindInPageIcon from "@material-ui/icons/FindInPage";

const useStyles = makeStyles((theme) => ({
  textField: {
    width: "20rem",
    "& ::-webkit-calendar-picker-indicator": {
      filter:
        "invert(100%) sepia(2%) saturate(7500%) hue-rotate(50deg) brightness(119%) contrast(111%)",
      fontSize: 26,
      "&:hover": {
        cursor: "pointer",
      },
    },
  },

  arrowIcon: {
    fill: "white",
    top: "16px",
  },
  filterPanelGrid: {
    backgroundColor: "rgba(0,0,50,0.55)",
    backdropFilter: "blur(10px)",
    alignItems: "center",
    borderRadius: "24px",
    marginBottom: "1em",
    justifyContent: "center",
    padding: "1.5em 0",
    border: "7px solid white",
  },
  menuPaper: {
    maxHeight: 100,
  },
  valueLabel: {
    color: "transparent",
    fontFamily: "Orbitron",
    fontWeight: 100,
    textShadow: "0 0 8px dodgerblue",
    top: "-20px",
    "& > span > span": {
      color: "white",
      fontWeight: 600,
      position: "relative",
      right: "27px",
      top: "35px",
    },
  },
  priceRangeLabel: {
    color: "White",
    fontFamily: "Orbitron",
    fontWeight: 500,
    display: "flex",
    alignItems: "center",
    marginBottom: "0.8em",
  },
  priceGrid: {
    width: "auto",
    height: "10.7rem",
    border: "2px dodgerblue solid",
    borderRadius: "12px",
    padding: "2em 2.3em 2.6em 1.5em",
    transition: "0.15s linear",
  },
  wrapper: {
    width: "auto",
    margin: "1em",
  },
  searchBtn: {
    border: "3px solid dodgerblue",
    borderRadius: "100px",
    fontFamily: "Orbitron",
    color: "white",
    width: "100%",
    height: "50px",
    outline: "3px solid white",
    outlineOffset: "3px",
    transition: "0.1s linear",
    "&:hover": {
      border: "3px solid white",
      filter: "drop-shadow(0 0 7px dodgerblue)",
      outline: "3px solid dodgerblue",
      color: "dodgerblue",
    },
    "&:active": {
      outline: "7px solid dodgerblue",
    },
  },
}));

const ShopFilter = (props) => {
  const css = useStyles();
  const shopNameRef = useRef();
  const serviceNameRef = useRef();
  const shopAddressRef = useRef();
  const idRef = useRef();

  const [workingHours, setWorkingHours] = useState([1, 24]);
  const [washingUnits, setWashingUnits] = useState(1);

  const [disabledFilters, setDisabledFilters] = useState(false);

  const workingHoursHandler = (event, newValue) => {
    setWorkingHours(newValue);
  };

  const washingUnitsHandler = (event, newValue) => {
    setWashingUnits(newValue);
  };

  const disableFitlersHandle = () => {
    idRef.current.value.length > 0
      ? setDisabledFilters(true)
      : setDisabledFilters(false);
  };

  const clockHours = [
    <option
      key={0}
      style={{
        backgroundColor: "whitesmoke",
      }}
    ></option>,
  ];

  for (let i = 1; i <= 24; i++) {
    clockHours.push(
      <option
        key={i}
        value={i}
        style={{
          backgroundColor: "whitesmoke",
          color: "dodgerblue",
          fontWeight: 500,
        }}
      >
        {i}
      </option>
    );
  }

  const submitHandler = () => {
    let shopName = shopNameRef.current.value;
    let serviceName = serviceNameRef.current.value;
    let shopAddress = shopAddressRef.current.value;
    let id = idRef.current.value;

    shopName = shopName !== "" ? `&CarWashName=${shopName}` : "";
    serviceName =
      serviceName !== "" ? `&ServiceNameOrDescription=${serviceName}` : "";
    shopAddress = shopAddress !== "" ? `&Address=${shopAddress}` : "";
    id = id !== "" ? `&CarWashShopId=${id}` : "";

    const filter =
      shopName +
      serviceName +
      shopAddress +
      id +
      `&MinimumAmountOfWashingUnits=${washingUnits}` +
      `&RequiredAndEarlierOpeningTime=${workingHours[0]}` +
      `&RequiredAndLaterClosingTime=${workingHours[1]}`;
    props.setFilterParams(filter);
  };

  const onEnterPressed = (e) => {
    if (e.key === "Enter") {
      submitHandler();
    }
  };

  return (
    <Grid container className={css.filterPanelGrid}>
      <Grid container item className={css.wrapper}>
        <Grid
          container
          item
          spacing={3}
          direction="column"
          justifyContent="space-between"
          style={{ width: "auto", marginRight: "1em" }}
        >
          <Grid item>
            <TextField
              disabled={disabledFilters}
              className={css.textField}
              autoComplete="off"
              autoCorrect="off"
              color="secondary"
              onKeyDown={(e) => {
                onEnterPressed(e);
              }}
              label={
                <Grid
                  container
                  alignItems="center"
                  style={{ bottom: "1em", position: "relative" }}
                >
                  <ShopIcon />
                  <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                    {disabledFilters ? "Disabled" : "Shop Name"}
                  </p>
                </Grid>
              }
              variant="outlined"
              inputRef={shopNameRef}
            />
          </Grid>

          <Grid item>
            <TextField
              disabled={disabledFilters}
              className={css.textField}
              autoComplete="off"
              autoCorrect="off"
              color="secondary"
              onKeyDown={(e) => {
                onEnterPressed(e);
              }}
              label={
                <Grid
                  container
                  alignItems="center"
                  style={{ bottom: "1em", position: "relative" }}
                >
                  <RoomIcon />
                  <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                    {disabledFilters ? "Disabled" : "Shop Address"}
                  </p>
                </Grid>
              }
              variant="outlined"
              inputRef={shopAddressRef}
            />
          </Grid>
        </Grid>
        <Grid item>
          <TextField
            disabled={disabledFilters}
            className={css.textField}
            multiline
            minRows={7}
            maxRows={7}
            autoComplete="off"
            autoCorrect="off"
            color="secondary"
            onKeyDown={(e) => {
              onEnterPressed(e);
            }}
            label={
              <Grid
                container
                alignItems="center"
                style={{ bottom: "1em", position: "relative" }}
              >
                <FindInPageIcon />
                <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                  {disabledFilters ? "Disabled" : "Service Name / Description"}
                </p>
              </Grid>
            }
            variant="outlined"
            inputRef={serviceNameRef}
          />
        </Grid>
      </Grid>
      <Grid container item className={css.wrapper}>
        <Grid
          container
          item
          direction="column"
          justifyContent="center"
          alignItems="center"
          className={css.priceGrid}
          style={{
            borderColor: `${disabledFilters ? "white" : "dodgerblue"}`,
            transition: "none",
          }}
        >
          <Typography className={css.priceRangeLabel} gutterBottom>
            <WatchIcon style={{ marginRight: "0.15em" }} />
            {disabledFilters ? "Disabled" : "Working Time"}
          </Typography>
          <Slider
            disabled={disabledFilters}
            style={{
              width: "200px",
            }}
            classes={{ valueLabel: css.valueLabel }}
            min={1}
            max={24}
            step={1}
            value={workingHours}
            onChange={workingHoursHandler}
            valueLabelDisplay="on"
          />
        </Grid>
        <Grid
          container
          item
          direction="column"
          justifyContent="center"
          alignItems="center"
          className={css.priceGrid}
          style={{
            marginLeft: "2em",
            borderColor: `${disabledFilters ? "white" : "dodgerblue"}`,
            transition: "none",
          }}
        >
          <Typography className={css.priceRangeLabel} gutterBottom>
            <ServiceIcon style={{ marginRight: "0.15em" }} />
            {disabledFilters ? "Disabled" : "Washing Units"}
          </Typography>
          <Slider
            disabled={disabledFilters}
            style={{
              width: "200px",
            }}
            classes={{ valueLabel: css.valueLabel }}
            min={1}
            max={50}
            step={1}
            value={washingUnits}
            onChange={washingUnitsHandler}
            valueLabelDisplay="on"
          />
        </Grid>
      </Grid>
      <Grid container item className={css.wrapper} direction="column">
        <Grid
          container
          item
          style={{
            width: "auto",
            marginBottom: "3.2rem",
            position: "relative",
            bottom: "0.5rem",
          }}
        >
          <TextField
            onChange={disableFitlersHandle}
            autoComplete="off"
            autoCorrect="off"
            color="secondary"
            onKeyDown={(e) => {
              onEnterPressed(e);
              if (
                e.keyCode !== 8 &&
                (e.keyCode < 48 || e.keyCode > 57) &&
                (e.keyCode < 96 || e.keyCode > 105)
              ) {
                e.preventDefault();
              }
            }}
            label={
              <Grid
                container
                alignItems="center"
                style={{ bottom: "1em", position: "relative" }}
              >
                <IdIcon />
                <p style={{ marginLeft: "3px", marginRight: "-9px" }}>ID</p>
              </Grid>
            }
            variant="outlined"
            inputRef={idRef}
            style={{ width: "150px" }}
          />
        </Grid>
        <Grid container item style={{ width: "auto", position: "relative" }}>
          <Button
            disableRipple
            className={css.searchBtn}
            onClick={submitHandler}
          >
            SEARCH
          </Button>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default ShopFilter;
