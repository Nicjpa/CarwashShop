import React, { Fragment, useState } from "react";
import { NavLink } from "react-router-dom";
import { useTheme, makeStyles } from "@material-ui/core/styles";
import Logo from "../images/CarWashLogoPNG.png";
import ShopIcon from "@material-ui/icons/Storefront";
import BookingtIcon from "@material-ui/icons/Receipt";
import DateTime from "./DateTime";

import {
  AppBar,
  Toolbar,
  useScrollTrigger,
  useMediaQuery,
  Button,
  IconButton,
  Grid,
  SwipeableDrawer,
  List,
  ListItem,
  ListItemText,
  Zoom,
  Slide,
  Typography,
} from "@material-ui/core";

import LogoutIcon from "@material-ui/icons/PowerSettingsNewSharp";
import MenuIcon from "@material-ui/icons/Menu";
import GraphicEqSharpIcon from "@material-ui/icons/GraphicEqSharp";

const useStyles = makeStyles((theme) => ({
  toolbarMargin: {
    ...theme.mixins.toolbar,
    marginBottom: "160px",
    transition: "0.2s ease-out",
  },
  appbar: {
    backgroundColor: "rgba(35,35,47,1)",
    zIndex: theme.zIndex.modal,
    borderBottom: "10px dodgerblue solid",
    borderTop: "10px dodgerblue solid",
    transition: "0.2s ease-out",
    boxShadow: "0 0 50px dodgerblue",

    left: 0,
  },
  navLink: {
    color: "white",
    textAlign: "center",
    textDecoration: "none",
    transition: "0.15s ease-out",

    "& #btnLabel": {
      transition: "0.15s ease-out",
      fontSize: 24,
      fontFamily: "Orbitron",
      fontWeight: 500,
    },

    "& #btnIcon": {
      transition: "0.15s ease-out",
      fontSize: 70,
      fontFamily: "Orbitron",
      color: "white",
    },
    "&:hover #btnIcon": {
      transition: "0.15s ease-out",
      color: "white",
      filter: "drop-shadow(0 0 0.5rem dodgerblue) ",
    },

    "&:hover #btnLabel": {
      transition: "0.15s ease-out",
      color: "white",
      filter: "drop-shadow(0 0 0.5rem dodgerblue) ",
    },

    "&:active #btnLabel": {
      color: "dodgerblue",
    },
  },

  activeNav: {
    transition: "0.15s ease-out",
    color: "white",
    backgroundColor: "transparent",

    "& #btnLabel": {
      transition: "0.15s ease-out",
      fontSize: 24,
      fontFamily: "Orbitron",
      fontWeight: 700,
      color: "dodgerblue",
      filter: "drop-shadow(0 0 1rem dodgerblue) ",
    },

    "&:hover #btnLabel": {
      transition: "0.15s ease-out",
      color: "dodgerblue",
      filter: "drop-shadow(0 0 0.5rem dodgerblue) ",
    },

    "&:active #btnLabel": {
      color: "white",
    },

    "& #btnIcon": {
      transition: "0.15s ease-out",
      color: "white",
      filter: "drop-shadow(0 0 0.5rem dodgerblue) ",
    },

    "&:hover #btnIcon": {
      color: "white",
    },

    "&:active #btnIcon": {
      color: "dodgerblue",
    },
  },

  logo: {
    width: "150px",
    margin: "0.5em 0",
  },

  logoutButton: {
    backgroundColor: "transparent",
    borderRadius: 50,
    padding: "7px 20px",
    fontFamily: "Orbitron",
    fontSize: 24,
    textAlign: "center",
    color: "white",
    border: "3px white solid",
    transition: "0.2s ease-out",
    [theme.breakpoints.down("1500")]: {
      padding: "1px 10px",
      fontSize: 18,
      borderWidth: "2px",
    },
    "&:hover": {
      borderColor: "red",
      color: "red",
    },
    "&:active": {
      borderColor: "black",
      color: "black",
    },
    "&:hover #logoutIcon2": {},
    "&:active #logoutIcon2": {
      color: "black",
      borderColor: "black",
      transition: "0.2s ease-out",
    },
    "&:active #logoutParagraph": {
      color: "red",
    },
  },
  logoutParagraph: {
    margin: 0,
    color: "white",
    transition: "0.2s ease-out",
  },

  logoutIconButton: {
    color: "white",
    "&:hover": {
      backgroundColor: "transparent",
    },
    "&:hover #logoutIcon": {
      color: "red",
    },
    "&:active #logoutIcon": {
      color: "black",
    },
  },
  logoutIcon: {
    color: "white",
    fontSize: "44px",
    transition: "0.15s ease-out",
    backgroundColor: "transparent",
  },
  navIcons: { color: "white", fontSize: 80 },
}));

function ElevationScroll(props) {
  const { children } = props;

  const trigger = useScrollTrigger({
    disableHysteresis: true,
    threshold: 0,
  });

  return React.cloneElement(children, {
    elevation: trigger ? 4 : 0,
  });
}

const Header = (props) => {
  const theme = useTheme();
  const css = useStyles();
  const role = props.role;

  const [openDrawer, setOpenDrawer] = useState(false);
  const matchedMDSM = useMediaQuery(theme.breakpoints.down("1500"));
  const navHidden = useMediaQuery(theme.breakpoints.down("1100"));

  const logoutHandler = () => {
    localStorage.removeItem("userName");
    localStorage.removeItem("token");
    localStorage.removeItem("role");

    props.setLogin(false);
  };

  const ownerLink = {
    text: "MANAGEMENT",
    path: "/management",
    icon: <GraphicEqSharpIcon className={css.navIcons} id="btnIcon" />,
    delay: "500ms",
  };

  const navLinks = [
    {
      text: "SHOPS",
      path: "/shops",
      icon: <ShopIcon className={css.navIcons} id="btnIcon" />,
      delay: "300ms",
    },
    {
      text: "BOOKINGS",
      path: "/bookings",
      icon: <BookingtIcon className={css.navIcons} id="btnIcon" />,
      delay: "400ms",
    },
  ];

  role === "Owner" && navLinks.push(ownerLink);

  const drawer = (
    <Fragment>
      <SwipeableDrawer
        open={openDrawer}
        onClose={() => setOpenDrawer(false)}
        onOpen={() => setOpenDrawer(true)}
        style={{ zIndex: 10 }}
      >
        <div className={css.toolbarMargin} />
        <List disablePadding>
          {navLinks.map((item, index) => (
            <ListItem
              key={index}
              divider
              button
              component={NavLink}
              activeClassName={css.activeSlideNav}
              to={item.path}
              selected={false}
              className={css.drawerItem}
            >
              <ListItemText disableTypography>{item.text}</ListItemText>
            </ListItem>
          ))}
        </List>
      </SwipeableDrawer>
      <IconButton
        className={css.drawerIconContainer}
        onClick={() => setOpenDrawer(!openDrawer)}
        disableRipple
        style={{ color: `${openDrawer ? "dodgerblue" : "white"}` }}
      >
        <MenuIcon className={css.drawerIcon} id="drawerIcon" />
      </IconButton>
    </Fragment>
  );

  const logoutIconButton = (
    <IconButton
      onClick={logoutHandler}
      className={css.logoutIconButton}
      disableRipple
    >
      <LogoutIcon className={css.logoutIcon} id="logoutIcon" />
    </IconButton>
  );

  const logoutButton = (
    <Button
      disableRipple
      className={css.logoutButton}
      direcetion="row"
      onClick={logoutHandler}
      startIcon={
        <LogoutIcon
          id="logoutIcon2"
          style={{
            fontSize: `${matchedMDSM ? "26px" : "40px"}`,
          }}
        />
      }
    >
      <p className={css.logoutParagraph} id="logoutParagraph">
        Logout
      </p>
    </Button>
  );

  return (
    <Fragment>
      {props.isLoggedIn && (
        <Slide direction="down" in={true} timeout={350}>
          <div>
            <ElevationScroll>
              <AppBar position="fixed" color="primary" className={css.appbar}>
                <Toolbar>
                  <Grid container justifyContent="center">
                    <Grid
                      container
                      style={{
                        alignItems: "center",
                        width: "1920px",
                        height: "200px",
                        justifyContent: `${
                          navHidden ? "space-between" : "space-around"
                        }`,
                      }}
                    >
                      {navHidden && drawer}
                      <Zoom
                        in={true}
                        timeout={400}
                        style={{ transitionDelay: "100ms" }}
                      >
                        <Grid container item justifyContent="center" lg={2}>
                          <DateTime />
                        </Grid>
                      </Zoom>
                      <Zoom
                        in={true}
                        timeout={400}
                        style={{ transitionDelay: "200ms" }}
                      >
                        <Grid container item justifyContent="center" lg={1}>
                          <img src={Logo} className={css.logo} />
                        </Grid>
                      </Zoom>

                      {!navHidden && (
                        <Grid
                          container
                          item
                          justifyContent="space-around"
                          alignItems="center"
                          lg={5}
                        >
                          {navLinks.map((x) => (
                            <Zoom
                              key={x.text}
                              in={true}
                              timeout={300}
                              style={{
                                transitionDelay: x.delay,
                              }}
                            >
                              <Grid
                                key={x.text}
                                item
                                component={NavLink}
                                className={css.navLink}
                                activeClassName={css.activeNav}
                                exact
                                to={x.path}
                              >
                                {x.icon}
                                <Typography id="btnLabel">{x.text}</Typography>
                              </Grid>
                            </Zoom>
                          ))}
                        </Grid>
                      )}
                      <Zoom
                        in={true}
                        timeout={300}
                        style={{ transitionDelay: "500ms" }}
                      >
                        <Grid
                          container
                          item
                          alignItems="center"
                          direction="column"
                          lg={2}
                        >
                          {navHidden ? logoutIconButton : logoutButton}
                        </Grid>
                      </Zoom>
                    </Grid>
                  </Grid>
                </Toolbar>
              </AppBar>
            </ElevationScroll>
            <div className={css.toolbarMargin} />
            <p style={{ color: "transparent" }}>DON'T TOUCH</p>
          </div>
        </Slide>
      )}
      <Grid container>{props.children}</Grid>
    </Fragment>
  );
};

export default Header;
