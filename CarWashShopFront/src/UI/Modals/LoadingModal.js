import { makeStyles } from "@material-ui/core/styles";
import Backdrop from "@material-ui/core/Backdrop";
import { Modal, Grid } from "@material-ui/core";
import CircularProgress from "@material-ui/core/CircularProgress";

const useStyles = makeStyles((theme) => ({
  backdrop: {
    display: "flex",
    top: 0,
    left: 0,
    width: "100%",
    zIndex: 2000,
    backgroundColor: "rgba(0,0,0,0.7)",
    justifyContent: "center",
    alignItems: "center",
    //backdropFilter: "blur(5px)",
  },
  bottom: {
    left: "49%",
    position: "absolute",
  },
  top: {
    color: "dodgerblue",
    animationDuration: "550ms",
    left: "49%",
    position: "absolute",
  },
}));

const LoadingModal = (props) => {
  const css = useStyles();

  return (
    <Modal
      className={css.backdrop}
      open={props.loading}
      BackdropComponent={Backdrop}
    >
      <Grid>
        <CircularProgress
          variant="determinate"
          className={css.bottom}
          size={50}
          thickness={4}
          value={100}
        />
        <CircularProgress
          variant="indeterminate"
          disableShrink
          className={css.top}
          size={50}
          thickness={4}
        />
      </Grid>
    </Modal>
  );
};

export default LoadingModal;
