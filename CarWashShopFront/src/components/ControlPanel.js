import { makeStyles } from "@material-ui/core/styles";
import { useState } from "react";

import { Grid, Slide, Button } from "@material-ui/core";

import ShopControlTable from "./DataTables/ShopControlTable";
import IncomeOverviewTable from "./DataTables/IncomeOverviewTable";

import LoadingModal from "../UI/Modals/LoadingModal";
import { useEffect } from "react";

const useStyles = makeStyles((theme) => ({
  filterPanelGrid: {
    backgroundColor: "rgba(0,0,50,0.55)",
    backdropFilter: "blur(10px)",
    alignItems: "center",
    borderRadius: "30px",
    marginBottom: "1em",
    justifyContent: "center",
    border: "7px solid white",
    boxShadow: "0 0 15px dodgerblue",
  },
  panelBtnGrid: {
    width: "50%",
    borderRadius: 0,
  },
  panelBtn: {
    color: "white",
    backgroundColor: "rgba(35,35,47,1)",
    width: "100%",
    fontFamily: "Orbitron",
    fontSize: 20,
    textShadow: "0 0 8px dodgerblue",
    letterSpacing: "0.4em",
    transition: "0.15s linear",
    borderBottom: "7px solid white",
    padding: "1rem",
    "&:hover": {
      backgroundColor: "dodgerblue",
      letterSpacing: "0.6em",
    },
    "&:active": {
      backgroundColor: "rgba(35,35,47,1)",
    },
  },
}));

const ControlPanel = (props) => {
  const css = useStyles();
  const [controlSwitch, setControlSwitch] = useState(true);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setTimeout(() => {
      setIsLoading(false);
    }, 400);
  }, []);

  return (
    <>
      <LoadingModal loading={isLoading} />
      <Slide direction="up" in={true} timeout={600}>
        <Grid container className={css.filterPanelGrid}>
          <Grid container item className={css.panelBtnGrid}>
            <Button
              disableRipple
              className={css.panelBtn}
              style={{
                borderRadius: "22px 0 0 0",
                borderRight: "3px solid white",
                backgroundColor: !controlSwitch && "dodgerblue",
                letterSpacing: !controlSwitch && "0.6em",
              }}
              onClick={() => {
                setControlSwitch(false);
              }}
            >
              Income Overview
            </Button>
          </Grid>
          <Grid container item className={css.panelBtnGrid}>
            <Button
              disableRipple
              className={css.panelBtn}
              style={{
                borderRadius: "0 22px 0 0",
                borderLeft: "4px solid white",
                backgroundColor: controlSwitch && "dodgerblue",
                letterSpacing: controlSwitch && "0.6em",
              }}
              onClick={() => {
                setControlSwitch(true);
              }}
            >
              Shop Control
            </Button>
          </Grid>
          <Grid container item>
            {controlSwitch && (
              <ShopControlTable style={{ transition: "5s linear" }} />
            )}
            {!controlSwitch && <IncomeOverviewTable />}
          </Grid>
        </Grid>
      </Slide>
    </>
  );
};

export default ControlPanel;
