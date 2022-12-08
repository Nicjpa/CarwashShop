import { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";

import { Collapse, Grid, Typography, Button } from "@material-ui/core";

import StorefrontIcon from "@material-ui/icons/Storefront";
import RoomIcon from "@material-ui/icons/Room";
import AccessTimeIcon from "@material-ui/icons/AccessTime";
import LocalCarWashIcon from "@material-ui/icons/LocalCarWash";

import Pagination from "../Pagination";

const useStyles = makeStyles((theme) => ({
  expand: {
    transform: "rotate(0deg)",
    marginLeft: "auto",
    color: "white",
    transition: theme.transitions.create("transform", {
      duration: theme.transitions.duration.shortest,
    }),
  },
  expandOpen: {
    transform: "rotate(180deg)",
  },
  container: {
    border: "7px solid white",
    borderRadius: "24px",
    backgroundColor: "rgba(0,0,50,0.55)",
    backdropFilter: "blur(10px)",
    transition: "0.12s linear",
  },
  cell: {
    width: "25%",
    alignItems: "center",
    justifyContent: "center",
    padding: "1em 0",
  },
  cellTitle: {
    width: "25%",
    flexDirection: "column",
    alignItems: "center",
  },
  titleIcon: {
    fontSize: 50,
    color: "white",
    filter: "drop-shadow(0 0 7px dodgerblue)",
  },
  titleText: {
    color: "dodgerblue",
    fontFamily: "Orbitron",
    fontWeight: 700,
  },
  shopInfo: {
    color: "white",
    fontFamily: "Orbitron",
    fontWeight: 100,
    userSelect: "none",
    textAlign: "center",
  },
  collapseWrapper: {
    width: "100%",
    borderTop: "2px dodgerblue dashed",
  },
  serviceCardTitles: {
    fontFamily: "Orbitron",
    color: "dodgerblue",
    fontWeight: 700,
    fontSize: 18,
  },
  serviceCardInfo: {
    fontFamily: "Orbitron",
    fontSize: 18,
    textShadow: "0 0 10px white",
  },
  bookNowBtn: {
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 16,
    border: "2px solid dodgerblue",
    borderRadius: "10px",
    padding: "0.7em",
    margin: "0.5em 0",
    textShadow: "0 0 10px dodgerblue",
    transition: "0.15s linear",
    "&:hover": {
      backgroundColor: "dodgerblue",
      boxShadow: "0 0 15px dodgerblue",
    },
  },
  clickableRow: {
    cursor: "pointer",
    "&:hover": {
      backgroundColor: "rgba(0,0,0,0.4)",
    },
  },
  cardContainer: {
    border: "4px white solid",
    width: "450px",
    height: "400px",
    borderRadius: "20px",
    boxShadow: "0 0 15px transparent",
    transition: "0.15s linear",
    margin: "2em",
    outline: "1px solid dodgerblue",
    outlineOffset: "5px",
    "&:hover": {
      border: "4px dodgerblue solid",
      outline: "7px solid white",
      boxShadow: "0 0 40px dodgerblue",
    },
  },
  noContentContainer: {
    padding: "5rem",

    justifyContent: "Center",
    alignItems: "center",
    flexDirection: "column",
    filter: "drop-shadow(0 0 0.3rem dodgerblue)",
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
  noContentSubtitle: {
    paddingTop: "0.4rem",
    width: "auto",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 20,
    fontWeight: 700,
    textShadow: "0 0 3px dodgerblue",
    letterSpacing: "0.75em",
    textAlign: "center",
  },
}));

export const Row = (props) => {
  const css = useStyles();
  const shop = props.shop;

  const shopOpeningTime =
    shop.openingTime.toString().length < 2
      ? `0${shop.openingTime}`
      : shop.openingTime;

  const shopClosingTime =
    shop.closingTime.toString().length < 2
      ? `0${shop.closingTime}`
      : shop.closingTime;

  const [expanded, setExpanded] = useState(false);

  const handleExpandClick = () => {
    setExpanded(!expanded);
  };

  const CreateBookingModalHandler = (serviceInfo) => {
    props.setPromptModal((prevValue) => {
      return {
        ...prevValue,
        shopId: shop.id,
        serviceId: serviceInfo.serviceId,
        shopName: shop.name,
        serviceName: serviceInfo.serviceName,
        servicePrice: serviceInfo.servicePrice,
        bool: true,
      };
    });
  };

  return (
    <Grid container item style={{ borderTop: "2px solid dodgerblue" }}>
      <Grid
        container
        item
        justifyContent="space-evenly"
        onClick={handleExpandClick}
        className={css.clickableRow}
        style={{ backgroundColor: `${expanded ? "rgba(0,0,0,0.4)" : ""}` }}
      >
        <Grid container item className={css.cell}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shop.name}
          </Typography>
        </Grid>
        <Grid container item className={css.cell}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shop.address}
          </Typography>
        </Grid>
        <Grid container item className={css.cell}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shop.amountOfWashingUnits}
          </Typography>
        </Grid>
        <Grid container item className={css.cell}>
          <Typography
            variant="h6"
            className={css.shopInfo}
            style={{
              transition: "0.08s linear",
              color: `${expanded ? "dodgerblue" : ""}`,
            }}
          >
            {shopOpeningTime} - {shopClosingTime}
          </Typography>
        </Grid>
      </Grid>
      <Collapse
        component={Grid}
        container
        classes={{
          wrapper: css.collapseWrapper,
        }}
        style={{
          color: "white",
          backgroundColor: "rgba(0,0,0,0.4)",
        }}
        in={expanded}
        timeout="auto"
        unmountOnExit
      >
        <Grid container justifyContent="center">
          <Grid
            container
            item
            style={{
              padding: "2em 0",
              justifyContent: "space-evenly",
              alignItems: "center",
            }}
          >
            {shop.services.map((x) => (
              <Grid key={x.id} container item className={css.cardContainer}>
                <Grid
                  container
                  justifyContent="space-between"
                  style={{
                    padding: "1em 2em",
                  }}
                >
                  <Grid
                    container
                    item
                    style={{ width: "auto", flexDirection: "column" }}
                  >
                    <Typography className={css.serviceCardTitles}>
                      Service Name
                    </Typography>
                    <Typography className={css.serviceCardInfo}>
                      {x.name}
                    </Typography>
                  </Grid>
                  <Grid
                    container
                    item
                    style={{
                      width: "auto",
                      flexDirection: "column",
                      alignItems: "flex-end",
                    }}
                  >
                    <Typography className={css.serviceCardTitles}>
                      Price
                    </Typography>
                    <Typography className={css.serviceCardInfo}>
                      $ {x.price}
                    </Typography>
                  </Grid>
                </Grid>
                <Grid
                  container
                  style={{
                    padding: "1em 2em",
                    flexDirection: "column",
                    alignItems: "center",
                  }}
                >
                  <Typography
                    className={css.serviceCardTitles}
                    style={{ marginBottom: "0.3em" }}
                  >
                    Description
                  </Typography>
                  <Grid container item justifyContent="center">
                    <Typography
                      style={{
                        overflowWrap: "break-word",
                        width: "100%",
                        textAlign: "left",
                        overflow: "auto",
                        height: "100px",
                        padding: "0 0.3em",
                        color: "white",
                        fontFamily: "Orbitron",
                        fontWeight: 100,
                        fontSize: 18,
                        marginBottom: "0.5em",
                        border: "2px solid dodgerblue",
                        borderRadius: "8px",
                      }}
                    >
                      {x.description}
                    </Typography>
                  </Grid>
                </Grid>
                <Grid
                  container
                  style={{
                    borderTop: "1px solid dodgerblue",
                    padding: "1em 2em",
                    justifyContent: "center",
                  }}
                >
                  <Button
                    disableRipple
                    className={css.bookNowBtn}
                    onClick={() => {
                      CreateBookingModalHandler({
                        serviceId: x.id,
                        serviceName: x.name,
                        servicePrice: x.price,
                      });
                    }}
                  >
                    Book Now
                  </Button>
                </Grid>
              </Grid>
            ))}
          </Grid>
        </Grid>
      </Collapse>
    </Grid>
  );
};

const ConsumerShopTable = (props) => {
  const css = useStyles();
  const allShops = props.shops.data;
  console.log(typeof allShops.value);
  return (
    <>
      {allShops.length > 0 && (
        <Grid container direction="column" className={css.container}>
          <Grid
            container
            style={{
              padding: "2.5em 0",
            }}
          >
            <Grid container item className={css.cellTitle}>
              <StorefrontIcon className={css.titleIcon} />
              <Typography variant="h5" className={css.titleText}>
                Shop Name
              </Typography>
            </Grid>
            <Grid container item className={css.cellTitle}>
              <RoomIcon className={css.titleIcon} />
              <Typography variant="h5" className={css.titleText}>
                Address
              </Typography>
            </Grid>
            <Grid container item className={css.cellTitle}>
              <LocalCarWashIcon className={css.titleIcon} />
              <Typography variant="h5" className={css.titleText}>
                Washing Units
              </Typography>
            </Grid>
            <Grid container item className={css.cellTitle}>
              <AccessTimeIcon className={css.titleIcon} />
              <Typography variant="h5" className={css.titleText}>
                Working Hours
              </Typography>
            </Grid>
            <Grid
              container
              item
              className={css.cellTitle}
              style={{ width: "4%" }}
            />
          </Grid>

          <Grid container>
            {allShops.map((x) => (
              <Row key={x.id} shop={x} setPromptModal={props.setPromptModal} />
            ))}
            <Grid
              container
              item
              justifyContent="center"
              style={{ borderTop: "2px solid dodgerblue", padding: "1em" }}
            >
              <Grid item>
                <Pagination
                  totalCountOfItems={props.totalCountOfItems}
                  pagination={props.pagination}
                  setPagination={props.setPaginations}
                />
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      )}
      {typeof allShops.value === "string" && (
        <Grid container direction="column" className={css.container}>
          <Grid container item className={css.noContentContainer}>
            <Grid container item className={css.noContentTitle}>
              NO SHOPS FOUND
            </Grid>
            <Grid item className={css.noContentSubtitle}>
              CHECK YOUR FILTERS
            </Grid>
          </Grid>
        </Grid>
      )}
    </>
  );
};

export default ConsumerShopTable;
