import { makeStyles } from "@material-ui/core/styles";
import { Grid, Typography, IconButton } from "@material-ui/core";
import CloseIcon from "@material-ui/icons/Close";

const useStyles = makeStyles((theme) => ({
  ownerName: {
    textAlign: "center",
    fontFamily: "Orbitron",
    fontSize: 20,
    fontWeight: 500,
    color: "white",
    textShadow: "0 0 8px dodgerblue",
  },
  ownerNameGrid: {
    width: "auto",
    border: "3px solid dodgerblue",
    borderRadius: "35px",
    padding: "1rem",
    justifyContent: "space-between",
    alignItems: "center",
    margin: "1rem 4rem",
  },
  iconBtn: {
    color: "white",
    border: "2px solid dodgerblue",
    padding: "0.2rem",
    transition: "0.15s linear",
    "&:hover": {
      backgroundColor: "red",
      borderColor: "red",
      boxShadow: "0 0 15px red",
    },
  },
}));

const CoownerNameRepresentation = (props) => {
  const css = useStyles();
  const reqDisbandMsg = props.bodyMessaging(
    `You will request disband from ${props.owner.toString()}.`
  );

  return (
    <Grid container item className={css.ownerNameGrid}>
      <Typography className={css.ownerName}>
        {props.owner.toUpperCase()}
      </Typography>
      <IconButton
        disableRipple
        className={css.iconBtn}
        onClick={() => {
          props.prompt(
            "disbandRequest",
            "Disband Request",
            reqDisbandMsg,
            null,
            null,
            {
              shopId: props.shopId,
              ownerName: props.owner,
              requestStatement: "",
            }
          );
        }}
      >
        <CloseIcon className={css.icon} />
      </IconButton>
    </Grid>
  );
};

export default CoownerNameRepresentation;
