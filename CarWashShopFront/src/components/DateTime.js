import { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Typography } from "@material-ui/core";

import AccountIcon from "@material-ui/icons/AccountCircleSharp";
import DateIcon from "@material-ui/icons/EventSharp";
import TimeIcon from "@material-ui/icons/ScheduleSharp";

const useStyles = makeStyles((theme) => ({
  gLeftContainerItem: {
    flexDirection: "column",
    width: "auto",
    height: "100%",
    justifyContent: "start",
    alignItems: "left",
    transition: "0.2s ease-out",
    marginTop: "0.1em",
    [theme.breakpoints.down("1100")]: {
      alignItems: "center",
      marginBottom: "3em",
    },
  },
  gItem: {
    color: "white",
    margin: "0.5em 0",
  },
  value: {
    color: "white",
    fontWeight: 500,
    textShadow: "0 0 5px dodgerblue",
  },
  label: {
    fontFamily: "Orbitron",
    fontWeight: 500,
    fontSize: 22,
  },
  icon: {
    verticalAlign: "bottom",
    marginRight: "0.2em",
    color: "white",
    borderRadius: "25px",
    border: "1px solid dodgerblue",
    boxShadow: "0 0 8px dodgerblue",
    fontSize: 40,
  },
}));

const DateTime = () => {
  const css = useStyles();
  const officerName = localStorage.getItem("userName");
  const [time, setTime] = useState(0);

  const currentDateTime = new Date();
  const dateValues = [
    currentDateTime.getDate(),
    currentDateTime.getMonth() + 1,
    currentDateTime.getHours(),
    currentDateTime.getMinutes(),
  ];
  let year = currentDateTime.getFullYear();

  const formatedDateValues = dateValues.map((x) => {
    return x < 10 ? `0${x}` : x;
  });

  let currentDate = `${formatedDateValues[0]}/${formatedDateValues[1]}/${year}`;
  let currentTime = `${formatedDateValues[2]}:${formatedDateValues[3]}`;

  useEffect(() => {
    const interval = setInterval(() => {
      setTime((value) => value + 1);
    }, 60000);
    return () => clearInterval(interval);
  }, []);

  return (
    <Grid container className={css.gLeftContainerItem}>
      <Grid container item className={css.gItem} alignItems="center">
        <Grid item>
          <AccountIcon className={css.icon} />
        </Grid>
        <Grid container item style={{ width: "auto" }}>
          <Typography className={css.label}>
            <span className={css.value}>{officerName}</span>
          </Typography>
        </Grid>
      </Grid>
      <Grid container item className={css.gItem}>
        <Grid item>
          <TimeIcon className={css.icon} />
        </Grid>
        <Grid container item style={{ width: "auto" }} alignItems="center">
          <Typography className={css.label}>
            <span className={css.value}>{currentTime}</span>
          </Typography>
        </Grid>
      </Grid>
      <Grid container item className={css.gItem}>
        <Grid item>
          <DateIcon className={css.icon} />
        </Grid>
        <Grid container item style={{ width: "auto" }} alignItems="center">
          <Typography className={css.label}>
            <span className={css.value}>{currentDate}</span>
          </Typography>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default DateTime;
