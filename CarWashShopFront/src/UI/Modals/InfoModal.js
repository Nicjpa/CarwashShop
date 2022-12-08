import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Modal, Backdrop, Fade, Grid, Typography } from "@material-ui/core";

import InfoIcon from "@material-ui/icons/NewReleasesOutlined";
import WarningIcon from "@material-ui/icons/ReportProblemOutlined";
import ErrorIcon from "@material-ui/icons/BlockOutlined";
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
    maxWidth: "55em",
    borderRadius: "0 0 2.5em 2.5em",
  },
  infoHeader: {
    backgroundColor: "dodgerblue",
    justifyContent: "center",
    padding: "0.3em 0 0.6em 0",
  },
  infoBody: {
    backgroundColor: "white",
    justifyContent: "center",
    padding: "3em",
    borderRadius: "0 0 1.8em 1.8em",
    flexDirection: "column",
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
  },
  icon: { fontSize: 70, color: "white", marginRight: "0.3em" },
}));

export default function InfoModal(props) {
  const css = useStyles();

  let themeColor = "";
  let icon;

  switch (props.params.themeColor) {
    case "info":
      themeColor = "dodgerblue";
      icon = <InfoIcon className={css.icon} />;
      break;
    case "warning":
      themeColor = "orange";
      icon = <WarningIcon className={css.icon} />;
      break;
    case "error":
      themeColor = "red";
      icon = <ErrorIcon className={css.icon} />;
      break;
    default:
      break;
  }

  const isArray = typeof props.params.modalDesc !== "string";

  return (
    <Modal
      className={css.modal}
      open={props.params.bool}
      onClose={() => {
        props.setModalBool((prevValues) => {
          return { ...prevValues, bool: false };
        });
      }}
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
      }}
    >
      <Fade in={props.params.bool}>
        <Grid className={css.paper} style={{ borderColor: themeColor }}>
          <Grid
            container
            item
            alignItems="center"
            className={css.infoHeader}
            style={{ backgroundColor: themeColor }}
          >
            {icon}
            <Typography variant="h3" className={css.modalTitle}>
              {props.params.modalTitle}
            </Typography>
          </Grid>

          <Grid
            container
            item
            className={css.infoBody}
            style={{ alignItems: `${isArray ? "flex-start" : "center"}` }}
          >
            {isArray &&
              props.params.modalDesc.map((x, i) => (
                <Grid item key={i} style={{ width: "100%" }}>
                  <Typography
                    variant="h6"
                    className={css.modalDesc}
                    style={{ color: themeColor }}
                  >
                    {x.replace(",", "")}
                  </Typography>
                </Grid>
              ))}
            {!isArray && (
              <Grid itemstyle={{ width: "100%" }}>
                <Typography
                  variant="h5"
                  className={css.modalDesc}
                  style={{ color: themeColor }}
                >
                  {props.params.modalDesc}
                </Typography>
              </Grid>
            )}
          </Grid>
        </Grid>
      </Fade>
    </Modal>
  );
}
