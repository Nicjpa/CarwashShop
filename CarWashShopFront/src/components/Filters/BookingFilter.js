import { makeStyles } from "@material-ui/core/styles";
import { useState, useRef } from "react";

import {
  Grid,
  TextField,
  FormControl,
  InputLabel,
  Select,
  Slider,
  Typography,
  Button,
} from "@material-ui/core";

import ShopIcon from "@material-ui/icons/Storefront";
import ServiceIcon from "@material-ui/icons/LocalCarWash";
import IdIcon from "@material-ui/icons/FormatListNumberedSharp";
import WatchIcon from "@material-ui/icons/AccessTimeSharp";
import DateConditionIcon from "@material-ui/icons/EventAvailableSharp";
import PriceIcon from "@material-ui/icons/MonetizationOnOutlined";
import BookingStatusIcon from "@material-ui/icons/StarSharp";
import ActiveInactiveIcon from "@material-ui/icons/SyncSharp";
import RoomIcon from "@material-ui/icons/Room";

const useStyles = makeStyles((theme) => ({
  textField: {
    minWidth: "100px",
    maxWidth: "300px",
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
    border: "7px solid white",
    padding: "2em",
    alignItems: "center",
    borderRadius: "24px",
    backdropFilter: "blur(10px)",
    justifyContent: "space-around",
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
    marginBottom: 0,
  },
  priceGrid: {
    width: "auto",
    border: "2px dodgerblue solid",
    borderRadius: "12px",
    padding: "1.4em 2.3em 2.2em 1.5em",
    transition: "0.15s linear",
  },
  wrapper: {
    width: "auto",
    borderRadius: "15px",
    margin: "1em",
  },
  papir: {
    maxHeight: "50px",
    height: "10px",
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

const BookingFilter = (props) => {
  const css = useStyles();
  const role = props.role;

  const shopNameRef = useRef();
  const serviceNameRef = useRef();
  const shopAddressRef = useRef();
  const dateConditionRef = useRef();
  const dateRef = useRef();
  const idRef = useRef();

  const statusRef = useRef();
  const stateRef = useRef();

  const [priceRange, setPriceRange] = useState([1, 100]);
  const [timeRange, setTimeRange] = useState([1, 24]);

  const [disabledFilters, setDisableFilters] = useState(false);

  const handlePriceChange = (event, newValue) => {
    setPriceRange(newValue);
  };

  const handleTimeChange = (event, newValue) => {
    setTimeRange(newValue);
  };

  const idSearchHandle = () => {
    idRef.current.value.length > 0
      ? setDisableFilters(true)
      : setDisableFilters(false);
  };

  const dateOptions = [
    { key: "", value: "" },
    { key: "Exact booked date", value: "&OnScheduledDate=" },
    { key: "Before booked date", value: "&ScheduledDatesBefore=" },
    { key: "After booked date", value: "&ScheduledDatesAfter=" },
  ];

  const statusOptions = [
    { key: "", value: "" },
    { key: "Pending", value: "&BookingStatus=1" },
    { key: "Confirmed", value: "&BookingStatus=2" },
    { key: "Rejected", value: "&BookingStatus=3" },
  ];

  const validityOptions = [
    { key: "", value: "" },
    { key: "Active", value: "&IsActiveBooking=true" },
    { key: "Inactive", value: "&IsActiveBooking=false" },
  ];

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
    let id = idRef.current.value;
    let shopName = shopNameRef.current.value;
    let serviceName = serviceNameRef.current.value;
    let shopAddress = shopAddressRef.current.value;
    let dateCondition = dateConditionRef.current.value;
    let date = dateRef.current.value;
    let status = statusRef.current.value;
    let state = stateRef.current.value;

    id = id !== "" ? `&BookingID=${id}` : "";

    shopName = shopName !== "" ? `&CarWashShopName=${shopName}` : "";
    serviceName = serviceName !== "" ? `&ServiceName=${serviceName}` : "";
    shopAddress = shopAddress !== "" ? `&ShopAddress=${shopAddress}` : "";
    dateCondition =
      date !== "" && dateCondition !== "" ? dateCondition + date : "";

    const filter =
      shopName +
      serviceName +
      shopAddress +
      dateCondition +
      id +
      status +
      state +
      `&MinPrice=${priceRange[0]}&MaxPrice=${priceRange[1]}` +
      `&ScheduledHoursAfter=${timeRange[0]}&ScheduledHoursBefore=${timeRange[1]}`;

    props.setFilterParams(filter);
  };

  const onEnterPressed = (e) => {
    if (e.key === "Enter") {
      submitHandler();
    }
  };

  return (
    <Grid container style={{ padding: "0.5em" }}>
      <Grid container className={css.filterPanelGrid}>
        <Grid
          container
          item
          direction="column"
          spacing={2}
          style={{ width: "auto" }}
        >
          <Grid item>
            <TextField
              className={css.textField}
              onChange={idSearchHandle}
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
              style={{ width: "8.6rem" }}
            />
          </Grid>
          <Grid item>
            <FormControl variant="outlined">
              <InputLabel>
                {
                  <Grid
                    container
                    alignItems="center"
                    style={{ bottom: "1em", position: "relative" }}
                  >
                    <BookingStatusIcon />
                    <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                      {disabledFilters ? "Disabled" : "Status"}
                    </p>
                  </Grid>
                }
              </InputLabel>
              <Select
                style={{ width: "8.6rem" }}
                disabled={disabledFilters}
                className={css.textField}
                native
                // defaultValue={
                //   role === "Owner"
                //     ? statusOptions[1].value
                //     : statusOptions[0].value
                // }
                label="Statuss_"
                color="secondary"
                inputProps={{
                  classes: {
                    icon: css.arrowIcon,
                  },
                }}
                inputRef={statusRef}
              >
                {statusOptions.map((x) => (
                  <option
                    key={x.key}
                    value={x.value}
                    style={{
                      backgroundColor: "whitesmoke",
                      color: "dodgerblue",
                      fontWeight: 500,
                    }}
                  >
                    {x.key}
                  </option>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item>
            <FormControl variant="outlined">
              <InputLabel>
                {
                  <Grid
                    container
                    alignItems="center"
                    style={{ bottom: "1em", position: "relative" }}
                  >
                    <ActiveInactiveIcon />
                    <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                      {disabledFilters ? "Disabled" : "State"}
                    </p>
                  </Grid>
                }
              </InputLabel>
              <Select
                disabled={disabledFilters}
                className={css.textField}
                style={{ width: "8.6rem" }}
                native
                label="Statee_"
                color="secondary"
                inputProps={{
                  classes: {
                    icon: css.arrowIcon,
                  },
                }}
                inputRef={stateRef}
              >
                {validityOptions.map((x) => (
                  <option
                    key={x.key}
                    value={x.value}
                    style={{
                      backgroundColor: "whitesmoke",
                      color: "dodgerblue",
                      fontWeight: 500,
                    }}
                  >
                    {x.key}
                  </option>
                ))}
              </Select>
            </FormControl>
          </Grid>
        </Grid>
        <Grid
          container
          item
          direction="column"
          spacing={2}
          style={{ width: "auto" }}
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
                  <ServiceIcon />
                  <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                    {disabledFilters ? "Disabled" : "Service Name"}
                  </p>
                </Grid>
              }
              variant="outlined"
              inputRef={serviceNameRef}
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
            <PriceIcon style={{ marginRight: "0.15em" }} />
            {disabledFilters ? "Disabled" : "Price Range"}
          </Typography>
          <Slider
            disabled={disabledFilters}
            style={{
              width: "200px",
            }}
            classes={{ valueLabel: css.valueLabel }}
            min={1}
            max={100}
            step={5}
            value={priceRange}
            onChange={handlePriceChange}
            valueLabelDisplay="on"
          />

          <Typography
            className={css.priceRangeLabel}
            gutterBottom
            style={{ marginTop: "2rem" }}
          >
            <WatchIcon style={{ marginRight: "0.15em" }} />
            {disabledFilters ? "Disabled" : "Time Range"}
          </Typography>
          <Slider
            disabled={disabledFilters}
            style={{
              width: "200px",
              marginBottom: "0.8rem",
            }}
            classes={{ valueLabel: css.valueLabel }}
            min={1}
            max={24}
            step={1}
            value={timeRange}
            onChange={handleTimeChange}
            valueLabelDisplay="on"
          />
        </Grid>
        <Grid
          container
          item
          direction="column"
          spacing={2}
          style={{ width: "auto", paddingBottom: "0.5rem" }}
        >
          <Grid item>
            <FormControl variant="outlined">
              <InputLabel>
                <Grid
                  container
                  alignItems="center"
                  style={{ bottom: "1em", position: "relative" }}
                >
                  <DateConditionIcon />
                  <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                    {disabledFilters ? "Disabled" : "Date Condition"}
                  </p>
                </Grid>
              </InputLabel>
              <Select
                style={{ width: "14rem" }}
                disabled={disabledFilters}
                className={css.textField}
                native
                label="Date Conditionn_"
                color="secondary"
                inputProps={{
                  classes: {
                    icon: css.arrowIcon,
                  },
                }}
                inputRef={dateConditionRef}
              >
                {dateOptions.map((x) => (
                  <option
                    key={x.key}
                    value={x.value}
                    style={{
                      backgroundColor: "whitesmoke",
                      color: "dodgerblue",
                      fontWeight: 500,
                    }}
                  >
                    {x.key}
                  </option>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item>
            <TextField
              type="date"
              style={{ width: "14rem" }}
              disabled={disabledFilters}
              inputRef={dateRef}
              variant="outlined"
              label={disabledFilters ? "Disabled" : "Booked Date"}
              className={css.textField}
              InputLabelProps={{
                shrink: true,
              }}
            />
          </Grid>
          <Grid item>
            <Button
              disableRipple
              onClick={submitHandler}
              className={css.searchBtn}
            >
              SEARCH
            </Button>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default BookingFilter;
