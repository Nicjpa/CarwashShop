import { makeStyles } from "@material-ui/core/styles";
import { Grid, Typography } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  value: {
    width: "50%",
    justifyContent: "center",
  },
  typography: {
    textAlign: "center",
    fontFamily: "Orbitron",
    fontWeight: 500,
    textShadow: "0 0 8px dodgerblue",
  },
}));

const IncomeRow = (props) => {
  const isDay = props.type === "1";
  const model = props.model;
  const css = useStyles();

  return (
    <Grid
      container
      style={{
        justifyContent: "center",
        color: "white",
        borderTop: "2px solid dodgerblue",
        padding: "0.7rem 0",
      }}
    >
      {isDay && (
        <Grid container item justifyContent="center">
          <Grid item style={{ width: "32%" }}>
            <Typography className={css.typography}>{model.calendar}</Typography>
          </Grid>
          <Grid item style={{ width: "32%" }}>
            <Typography className={css.typography}>{model.date}</Typography>
          </Grid>
          <Grid item style={{ width: "32%" }}>
            <Typography className={css.typography}>{model.income}</Typography>
          </Grid>
        </Grid>
      )}
      {!isDay && (
        <Grid container item justifyContent="center">
          <Grid item style={{ width: "32%" }}>
            <Typography className={css.typography}>{model.calendar}</Typography>
          </Grid>
          <Grid item style={{ width: "32%" }}>
            <Typography className={css.typography}>{model.income}</Typography>
          </Grid>
        </Grid>
      )}
    </Grid>
  );
};

export default IncomeRow;
