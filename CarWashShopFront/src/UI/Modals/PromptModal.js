import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import {
  Modal,
  Backdrop,
  Fade,
  Grid,
  Typography,
  Button,
} from "@material-ui/core";

import WarningIcon from "@material-ui/icons/ReportProblemOutlined";

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
    justifyContent: "space-evenly",
    alignItems: "center",
    padding: "3em",
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
    color: "orange",
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
    color: "orange",
    borderTop: "4px solid orange",
    transition: "0.2s linear",
    "&:hover": {
      backgroundColor: "orange",
      color: "white",
    },
  },
  propValues: {
    width: "auto",
    borderTop: "4px solid orange",
    borderBottom: "4px solid orange",
    borderRadius: "15px",
    padding: "0.5em",
    margin: "2em",
  },
}));

export default function PromptModal(props) {
  const css = useStyles();

  const closeModalHandle = () => {
    props.closeModal((prevValues) => {
      return { ...prevValues, bool: false };
    });
  };

  return (
    <Modal
      className={css.modal}
      open={props.promptModal.bool}
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
      }}
    >
      <Fade in={props.promptModal.bool}>
        <Grid className={css.paper} style={{ borderColor: "orange" }}>
          <Grid
            container
            item
            alignItems="center"
            className={css.infoHeader}
            style={{ backgroundColor: "orange" }}
          >
            <WarningIcon className={css.icon} />
            <Typography variant="h3" className={css.modalTitle}>
              {props.promptModal.title}
            </Typography>
          </Grid>

          <Grid container item className={css.infoBody}>
            {props.promptModal.body}
          </Grid>
          <Grid container item className={css.btnGrid}>
            <Grid item style={{ width: "50%", height: "3.5em" }}>
              <Button
                className={css.btn}
                disableRipple
                onClick={async () => {
                  await props.executeYes();
                }}
                style={{
                  borderRight: "2px orange solid",
                  borderRadius: " 0 0 0 20px",
                  fontWeight: 700,
                }}
              >
                YES
              </Button>
            </Grid>
            <Grid item style={{ width: "50%", height: "3.5em" }}>
              <Button
                className={css.btn}
                disableRipple
                onClick={closeModalHandle}
                style={{
                  borderLeft: "2px orange solid",
                  borderRadius: " 0 0 20px 0",
                  fontWeight: 700,
                }}
              >
                NO
              </Button>
            </Grid>
          </Grid>
        </Grid>
      </Fade>
    </Modal>
  );
}
