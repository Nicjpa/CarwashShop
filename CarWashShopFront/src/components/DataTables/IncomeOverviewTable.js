import { makeStyles } from "@material-ui/core/styles";
import { useState, useEffect, useRef } from "react";
import {
  Grid,
  TextField,
  FormControl,
  InputLabel,
  Select,
  Radio,
  RadioGroup,
  FormControlLabel,
  Button,
  Typography,
  Zoom,
} from "@material-ui/core";

import StorefrontIcon from "@material-ui/icons/Storefront";
import CalendarTodayIcon from "@material-ui/icons/CalendarToday";
import AccountBalanceSharpIcon from "@material-ui/icons/AccountBalanceSharp";
import DateRangeSharpIcon from "@material-ui/icons/DateRangeSharp";
import HourglassEmptySharpIcon from "@material-ui/icons/HourglassEmptySharp";

import InfoModal from "../../UI/Modals/InfoModal";
import PromptModal from "../../UI/Modals/PromptModal";
import Pagination from "../Pagination";
import IncomeRow from "../TableRows/IncomeRow";
import { HTTPRequest } from "../../HTTPRequest";

const useStyles = makeStyles((theme) => ({
  filterPanel: {},
  head: {},
  body: {},
  foot: {},
  elementGrid: { justifyContent: "center" },
  elementBox: {
    width: "33%",

    justifyContent: "center",
    alignItems: "center",
    color: "white",
    padding: "1rem",
  },
  shopYearEl: { width: "auto", justifyContent: "center" },
  dateEl: { width: "auto" },
  radioEl: { width: "auto" },
  btnGrid: {
    justifyContent: "center",
    color: "white",
    borderTop: "7px solid white",
  },
  date: {
    minWidth: "100px",
    maxWidth: "300px",
    "& ::-webkit-calendar-picker-indicator": {
      filter:
        "invert(100%) sepia(2%) saturate(7500%) hue-rotate(50deg) brightness(119%) contrast(111%)",
      fontSize: 34,
      "&:hover": {
        cursor: "pointer",
      },
    },
  },
  arrowIcon: {
    fill: "white",
    top: "16px",
  },
  radioLabel: {
    "& .MuiTypography-body1": {
      fontFamily: "Orbitron",
      fontWeight: 500,
    },
  },
  radio: {
    color: "white",
  },
  btn: {
    width: "100%",
    backgroundColor: "rgba(35,35,47,1)",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 24,
    fontWeight: 500,
    letterSpacing: "0.2rem",
    textShadow: "0 0 8px dodgerblue",
    padding: "0.8rem",
    borderBottom: "7px solid white",
    borderRadius: 0,
    transition: "0.15s linear",
    "&:hover": {
      backgroundColor: "dodgerblue",
      letterSpacing: "0.5rem",
    },
    "&:active": {
      backgroundColor: "rgba(35,35,47,1)",
    },
  },
  thTitleGrid: {
    width: "32%",
    justifyContent: "center",
    alignItems: "center",
    padding: "1.5rem",
    flexDirection: "column",
  },
  thTitle: {
    fontFamily: "Orbitron",
    color: "dodgerblue",
    fontSize: 24,
    fontWeight: 500,
    textShadow: "0 0 8px dodgerblue",
  },
  thIcon: {
    color: "white",
    fontSize: 50,
    filter: "drop-shadow(0 0 10px dodgerblue)",
  },
  errorRed: {
    "& .MuiInputBase-input": {
      color: "red",
    },
  },
  noContentSubtitle: {
    paddingTop: "0.4rem",
    width: "auto",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 20,
    fontWeight: 700,
    textShadow: "0 0 3px dodgerblue",
    letterSpacing: "1.1em",
    textAlign: "center",
  },
  noContentTitle: {
    width: "auto",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 50,
    fontWeight: 700,
    textShadow: "0 0 2px dodgerblue",
    borderBottom: "4px solid dodgerblue",
  },
  noContentContainer: {
    padding: "5rem",
    justifyContent: "Center",
    alignItems: "center",
    flexDirection: "column",
    filter: "drop-shadow(0 0 0.3rem dodgerblue)",
  },
}));

const IncomeOverviewTable = () => {
  const css = useStyles();
  const startDateRef = useRef();
  const endDateRef = useRef();

  const [shopSelectedValue, setShopSelectedValue] = useState("");
  const [yearValue, setYearValue] = useState(new Date().getFullYear());

  const [type, setType] = useState("1");
  const [income, setIncome] = useState([]);
  const [shops, setShops] = useState([]);

  const [yearError, setYearError] = useState(false);
  const [shopError, setShopError] = useState(false);

  const [pagination, setPaginations] = useState({
    currentPage: 1,
    recordsPerPage: 10,
  });

  const handleChange = (event) => {
    setType(event.target.value);
  };

  const getMoney = async () => {
    const shopValue = shopSelectedValue;
    const startDateValue = startDateRef.current.value;
    const endDateValue = endDateRef.current.value;

    let filter = `CarWashShopID=${shopValue}&CalendarFormat=${type}&ForTheYear=${yearValue}`;

    filter +=
      startDateValue.trim().length > 0 ? `&StartingDate=${startDateValue}` : "";

    filter +=
      endDateValue.trim().length > 0 ? `&EndingDate=${endDateValue}` : "";

    const httpParams = {
      controller: "OwnerManagement/",
      action: "GetIncomeReport?",
      method: "GET",
      params: filter,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const isYearValid = yearValue.toString().length === 4;
    const isShopValid = parseInt(shopValue) > 0;

    if (isYearValid && isShopValid) {
      isShopValid && setShopError(false);
      isYearValid && setYearError(false);
      try {
        const response = await HTTPRequest(httpParams);

        const noIncomeMsg = response.data.value;
        if (noIncomeMsg === undefined) {
          setIncome([...response.data]);
        } else {
          setIncome(() => {
            return [];
          });
        }
      } catch (error) {}
    } else {
      !isShopValid && setShopError(true);
      !isYearValid && setYearError(true);
    }
  };

  const getShops = async () => {
    const httpParams = {
      controller: "OwnerManagement/",
      action: `GetShopOwners?Page=${pagination.currentPage}&RecordsPerPage=50`,
      method: "GET",
      params: null,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    try {
      const response = await HTTPRequest(httpParams);

      const shopIds = response.data.map((x) => {
        return { id: x.id, shopName: x.name };
      });

      setShops([...shopIds]);
    } catch (error) {}
  };

  useEffect(() => {
    shops.length === 0 ? getShops() : getMoney();
  }, [shopSelectedValue, type, yearValue]);

  let incomeFormat;
  let yearFormat;

  switch (type) {
    case "2":
      incomeFormat = "Weekly";
      yearFormat = "Week of a year";
      break;
    case "3":
      incomeFormat = "Monthly";
      yearFormat = "Month of a year";
      break;
    case "4":
      incomeFormat = "Yearly";
      yearFormat = "Year";
      break;
    default:
      break;
  }

  console.log(income);

  return (
    <Zoom
      in={true}
      timeout={500}
      style={{
        transitionDelay: "100ms",
      }}
    >
      <Grid container>
        <Grid container item className={css.filterPanel}>
          <Grid container item className={css.elementGrid}>
            <Grid container item className={css.elementBox}>
              <Grid
                container
                item
                className={css.shopYearEl}
                style={{ width: "100%" }}
              >
                <FormControl variant="outlined">
                  <InputLabel>
                    <Grid
                      container
                      item
                      style={{ bottom: "0.3rem", position: "relative" }}
                    >
                      <StorefrontIcon style={{ color: shopError && "red" }} />
                      <Typography
                        style={{
                          fontFamily: "Orbitron",
                          fontWeight: 500,
                          color: shopError && "red",
                        }}
                      >
                        Your Shops
                      </Typography>
                    </Grid>
                  </InputLabel>
                  <Select
                    value={shopSelectedValue}
                    onChange={(e) => {
                      setShopSelectedValue(e.target.value);
                    }}
                    style={{ width: "14rem" }}
                    className={css.date}
                    native
                    error={shopError}
                    onFocus={() => {
                      setShopError(false);
                    }}
                    label="Shop Nameiii_"
                    color="secondary"
                    inputProps={{
                      classes: {
                        icon: css.arrowIcon,
                      },
                    }}
                  >
                    <option
                      value={""}
                      style={{
                        backgroundColor: "whitesmoke",
                        color: "dodgerblue",
                        fontWeight: 500,
                      }}
                    ></option>
                    {shops.map((x, i) => (
                      <option
                        key={x.id}
                        value={x.id}
                        style={{
                          backgroundColor: "whitesmoke",
                          color: "dodgerblue",
                          fontWeight: 500,
                        }}
                      >
                        {x.shopName}
                      </option>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
              <Grid container item className={css.shopYearEl}>
                <TextField
                  className={`${yearError && css.errorRed}`}
                  type="text"
                  style={{ width: "14rem" }}
                  error={yearError}
                  value={yearValue}
                  onChange={(e) => {
                    setYearValue(e.target.value);
                  }}
                  inputProps={{ maxLength: 4 }}
                  autoComplete="off"
                  autoCorrect="off"
                  color="secondary"
                  onFocus={() => {
                    setYearError(false);
                  }}
                  onKeyDown={(e) => {
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
                      style={{ bottom: "1.1rem", position: "relative" }}
                    >
                      <CalendarTodayIcon />
                      <p style={{ marginLeft: "3px", marginRight: "-9px" }}>
                        {"Year"}
                      </p>
                    </Grid>
                  }
                  variant="outlined"
                />
              </Grid>
            </Grid>
            <Grid container item className={css.elementBox}>
              <Grid container item className={css.dateEl}>
                <TextField
                  style={{ width: "14rem" }}
                  inputRef={startDateRef}
                  type="date"
                  variant="outlined"
                  label="From"
                  className={css.date}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
              </Grid>
              <Grid container item className={css.dateEl}>
                <TextField
                  style={{ width: "14rem" }}
                  inputRef={endDateRef}
                  type="date"
                  variant="outlined"
                  label="To"
                  className={css.date}
                  InputLabelProps={{
                    shrink: true,
                  }}
                />
              </Grid>
            </Grid>

            <Grid
              container
              item
              direction="column"
              className={css.elementBox}
              style={{ width: "20%" }}
            >
              <Grid container item className={css.radioEl}>
                <FormControl>
                  <RadioGroup value={type} onChange={handleChange}>
                    <FormControlLabel
                      className={css.radioLabel}
                      value="1"
                      control={<Radio className={css.radio} />}
                      label="Day"
                      style={{
                        color: `${type === "1" ? "dodgerblue" : "white"}`,
                      }}
                    />
                    <FormControlLabel
                      className={css.radioLabel}
                      value="2"
                      control={<Radio className={css.radio} />}
                      label="Week"
                      style={{
                        color: `${type === "2" ? "dodgerblue" : "white"}`,
                      }}
                    />
                    <FormControlLabel
                      className={css.radioLabel}
                      value="3"
                      control={<Radio className={css.radio} />}
                      label="Month"
                      style={{
                        color: `${type === "3" ? "dodgerblue" : "white"}`,
                      }}
                    />
                    <FormControlLabel
                      className={css.radioLabel}
                      value="4"
                      control={<Radio className={css.radio} />}
                      label="Year"
                      style={{
                        color: `${type === "4" ? "dodgerblue" : "white"}`,
                      }}
                    />
                  </RadioGroup>
                </FormControl>
              </Grid>
            </Grid>
          </Grid>
          <Grid container item className={css.btnGrid}>
            <Button disableRipple className={css.btn} onClick={getMoney}>
              CHECK INCOME
            </Button>
          </Grid>
        </Grid>
        {income.length > 0 ? (
          <>
            <Grid container item className={css.head} justifyContent="center">
              {type === "1" ? (
                <>
                  <Grid container item className={css.thTitleGrid}>
                    <HourglassEmptySharpIcon className={css.thIcon} />
                    <Typography className={css.thTitle}>
                      Day in a year
                    </Typography>
                  </Grid>
                  <Grid container item className={css.thTitleGrid}>
                    <DateRangeSharpIcon className={css.thIcon} />
                    <Typography className={css.thTitle}>Date</Typography>
                  </Grid>
                  <Grid container item className={css.thTitleGrid}>
                    <AccountBalanceSharpIcon className={css.thIcon} />
                    <Typography className={css.thTitle}>
                      Daily income
                    </Typography>
                  </Grid>
                </>
              ) : (
                <>
                  <Grid container item className={css.thTitleGrid}>
                    <AccountBalanceSharpIcon className={css.thIcon} />
                    <Typography className={css.thTitle}>
                      {yearFormat}
                    </Typography>
                  </Grid>
                  <Grid container item className={css.thTitleGrid}>
                    <AccountBalanceSharpIcon className={css.thIcon} />
                    <Typography className={css.thTitle}>
                      {incomeFormat} income
                    </Typography>
                  </Grid>
                </>
              )}
            </Grid>

            <Grid container item className={css.body}>
              {income.length > 0 &&
                income.map((x, i) => (
                  <IncomeRow key={i} model={x} type={type} />
                ))}
            </Grid>
          </>
        ) : (
          <Zoom
            in={true}
            timeout={500}
            style={{
              transitionDelay: "100ms",
            }}
          >
            <Grid container item className={css.noContentContainer}>
              <Grid container item className={css.noContentTitle}>
                NO REGISTERED INCOME
              </Grid>
              <Grid item className={css.noContentSubtitle}>
                GROW YOUR BUSINESS
              </Grid>
            </Grid>
          </Zoom>
        )}
      </Grid>
    </Zoom>
  );
};

export default IncomeOverviewTable;
