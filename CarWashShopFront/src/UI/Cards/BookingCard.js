import { useState } from "react";
import { HTTPRequest } from "../../HTTPRequest";
import { makeStyles } from "@material-ui/core/styles";
import {
  Grid,
  Card,
  CardContent,
  Typography,
  IconButton,
  Collapse,
  Button,
  Tooltip,
} from "@material-ui/core";
import BookingIcon from "@material-ui/icons/Receipt";
import StorefrontIcon from "@material-ui/icons/Storefront";
import LocalCarWashIcon from "@material-ui/icons/LocalCarWash";
import BookingStatusIcon from "@material-ui/icons/StarSharp";
import RoomIcon from "@material-ui/icons/Room";
import AccessTimeIcon from "@material-ui/icons/AccessTime";
import EventIcon from "@material-ui/icons/Event";
import CloseSharpIcon from "@material-ui/icons/CloseSharp";

import FormatListNumberedIcon from "@material-ui/icons/FormatListNumbered";
import ScheduleIcon from "@material-ui/icons/Schedule";
import AlternateEmailSharpIcon from "@material-ui/icons/AlternateEmailSharp";
import PersonSharpIcon from "@material-ui/icons/PersonSharp";
import AccountCircleSharpIcon from "@material-ui/icons/AccountCircleSharp";
import PhoneEnabledSharpIcon from "@material-ui/icons/PhoneEnabledSharp";

import ThumbDownSharpIcon from "@material-ui/icons/ThumbDownSharp";
import ThumbUpSharpIcon from "@material-ui/icons/ThumbUpSharp";

const useStyles = makeStyles((theme) => ({
  cardContainer: {
    margin: "1em",
    backgroundColor: "rgba(0,0,50,0.55)",
    outline: "3px white solid",
    borderRadius: "10px",
    backdropFilter: "blur(10px)",
    transition: "0.12s linear",

    "&:hover": {
      outline: "8px dodgerblue solid",
      backgroundColor: "rgba(0,0,50,0.7)",
    },
  },

  cardHeader: {
    color: "white",
    padding: "1.5em",
    borderBottom: "2px dodgerblue solid",
  },
  cardBody: {
    color: "white",
    alignItems: "center",
    maxHeight: "13.3rem",
    "&.MuiCardContent-root": {
      paddingTop: 0,
      paddingBottom: 0,
      margin: 0,
    },
  },
  cardFooter: {
    color: "white",
    padding: "1em 1.5em",
    borderTop: "2px dodgerblue solid",
    "&.MuiCardContent-root:last-child": {
      paddingBottom: "1em",
    },
  },
  state: {
    fontFamily: "Orbitron",
    color: "white",
    fontSize: 20,
    fontWeight: 500,
    textShadow: "0 0 10px dodgerblue",
    border: "3px solid dodgerblue",
    borderRadius: "15px",
    padding: "0.3em 0.7em",
  },
  key: {
    fontFamily: "Orbitron",
    color: "dodgerblue",
    fontWeight: 500,
    fontSize: 20,
  },
  value: {
    fontFamily: "Orbitron",
    color: "white",
    fontWeight: 500,
    fontSize: 22,
    textShadow: "0 0 8px dodgerblue",
  },
  infoText: {
    fontFamily: "Orbitron",
    fontSize: 16,
    fontWeight: 500,
    color: "white",
    textShadow: "0 0 8px dodgerblue",
    overflowWrap: "break-word",
  },
  infoType: {
    fontFamily: "Orbitron",
    fontSize: 18,
    fontWeight: 500,
    color: "dodgerblue",
  },
  icon: {
    fontSize: 55,
    color: "white",
    filter: "drop-shadow(0 0 4px dodgerblue)",
  },
  column: {
    width: "auto",
    margin: "2em 0",
  },
  button: {
    padding: "0.1em",
    border: "3px dodgerblue solid",
    transition: "0.12s linear",
    "&:hover": {
      backgroundColor: "red",
      border: "3px red solid",
    },
    "& #cancelIcon": {
      fontSize: 50,
      color: "white",
    },
  },
  thumbButtons: {
    fontSize: 60,
    padding: "0.1em",
    border: "3px dodgerblue solid",
    borderRadius: "50px",
    transition: "0.12s linear",
    cursor: "pointer",
    transition: "0.15s linear",
  },

  thumbReject: {
    transition: "0.15s linear",
    "&:hover": {
      border: "3px red solid",
      color: "red",
    },
    "&:active": {
      transition: "0.05s linear",
      border: "8px red solid",
    },
  },
  thumbConfirm: {
    transition: "0.15s linear",
    "&:hover": {
      border: "3px limegreen solid",
      color: "limegreen",
    },
    "&:active": {
      transition: "0.05s linear",
      border: "8px limegreen solid",
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
  collapseWrapper: {
    width: "100%",
  },
  collapseBtn: {
    fontFamily: "Orbitron",
    fontSize: 24,
    fontWeight: 500,
    padding: 0,
    textAlign: "center",
    backgroundColor: "white",
    width: "100%",
    color: "dodgerblue",
    borderRadius: 0,
    transition: "0.15s linear",
    marginTop: "1em",
    "&:hover": {
      fontWeight: 900,
      color: "white",
      backgroundColor: "dodgerblue",
      letterSpacing: "0.4em",
      filter: "drop-shadow(0 0 10px dodgerblue)",
    },
  },

  expandedBtn: {
    fontWeight: 900,
    color: "white",
    backgroundColor: "dodgerblue",
    transition: "0.15s linear",
    letterSpacing: "0.4em",
    filter: "drop-shadow(0 0 10px dodgerblue)",
  },
}));

export default function OwnerBookingCard(props) {
  const css = useStyles();
  const booking = props.booking;
  const role = props.role;
  const [expanded, setExpanded] = useState();

  const changeBookingStatusHadnler = async (bookingStatus) => {
    const bookingStatusParams = {
      controller: "OwnerManagement/",
      action: `ConfirmRejectBooking?BookingId=${booking.id}`,
      method: "PUT",
      params: `&BookingStatus=${bookingStatus}`,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    try {
      await HTTPRequest(bookingStatusParams);
      let status = bookingStatus === 2 ? "Confirmed" : "Rejected";
      props.setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Booking status",
          modalDesc: `Booking #${booking.id} is ${status}!`,
          themeColor: "info",
        };
      });
      props.reloadPage();
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      const modalTitle =
        errorMessage === "No server response"
          ? "Connection problem"
          : "Booking status error";

      props.setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: modalTitle,
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
  };

  const cancelBookingBody = (
    <>
      <Grid container item alignItems="center" className={css.propValues}>
        <FormatListNumberedIcon style={{ color: "orange", fontSize: 40 }} />
        <Typography
          style={{
            color: "orange",
            fontFamily: "Orbitron",
            fontWeight: 500,
            fontSize: 26,
          }}
        >
          {booking.id}
        </Typography>
      </Grid>
      <Grid container item alignItems="center" className={css.propValues}>
        <EventIcon style={{ color: "orange", fontSize: 40 }} />
        <Typography
          style={{
            color: "orange",
            fontFamily: "Orbitron",
            fontWeight: 500,
            fontSize: 26,
          }}
        >
          {booking.scheduledDate}
        </Typography>
      </Grid>
      <Grid container item alignItems="center" className={css.propValues}>
        <ScheduleIcon style={{ color: "orange", fontSize: 40 }} />
        <Typography
          style={{
            color: "orange",
            fontFamily: "Orbitron",
            fontWeight: 500,
            fontSize: 26,
          }}
        >
          {booking.scheduledTime.length < 5
            ? `0${booking.scheduledTime}`
            : booking.scheduledTime}
        </Typography>
      </Grid>
    </>
  );

  const promptCancelingHandle = () => {
    props.cancelModal((prevValues) => {
      return {
        ...prevValues,
        bool: true,
        body: cancelBookingBody,
        id: booking.id,
      };
    });
  };

  return (
    <Card className={css.cardContainer}>
      <CardContent className={css.cardHeader}>
        <Grid container justifyContent="space-between">
          <Grid
            container
            item
            alignItems="center"
            spacing={1}
            style={{ width: "auto" }}
          >
            <Grid container item style={{ width: "auto" }}>
              <BookingIcon
                style={{
                  fontSize: 60,
                  color: "white",
                  filter: "drop-shadow(0 0 4px dodgerblue)",
                }}
              />
            </Grid>
            <Grid
              container
              item
              direction="column"
              className={css.key}
              style={{ width: "auto" }}
            >
              <Typography className={css.key}>Booking ID</Typography>
              <Typography className={css.value}>#{booking.id}</Typography>
            </Grid>
          </Grid>

          {role === "Consumer" && (
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: "center",
                marginRight: "0.4em",
              }}
            >
              {booking.isPaid || booking.bookingStatus === "Rejected" ? (
                <Typography
                  className={css.state}
                  style={{
                    border: `${
                      booking.bookingStatus === "Rejected" && "2px solid white"
                    }`,
                    backgroundColor: `${
                      booking.bookingStatus === "Rejected" && "red"
                    }`,
                    color: `${booking.bookingStatus === "Rejected" && "white"}`,
                    textShadow: `${
                      booking.bookingStatus === "Rejected" && "none"
                    }`,
                  }}
                >
                  {booking.isPaid ? "Inactive" : "Rejected"}
                </Typography>
              ) : (
                <IconButton
                  className={css.button}
                  disableRipple
                  onClick={promptCancelingHandle}
                >
                  <CloseSharpIcon id="cancelIcon" />
                </IconButton>
              )}
            </Grid>
          )}
          {role === "Owner" && (
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: "center",
                marginRight: "0.4em",
              }}
            >
              {booking.bookingStatus === "Confirmed" &&
                (booking.isPaid ? (
                  <Typography className={css.state}>Inactive</Typography>
                ) : (
                  <Tooltip title="Reject Booking" arrow>
                    <ThumbDownSharpIcon
                      className={`${css.thumbButtons} ${css.thumbReject}`}
                      onClick={() => {
                        changeBookingStatusHadnler(3);
                      }}
                    />
                  </Tooltip>
                ))}
              {booking.bookingStatus === "Pending" && (
                <Grid container item spacing={2} style={{ width: "auto" }}>
                  <Grid
                    container
                    item
                    alignItems="center"
                    style={{ width: "auto" }}
                  >
                    <Tooltip title="Confirm Booking" arrow>
                      <ThumbUpSharpIcon
                        className={`${css.thumbButtons} ${css.thumbConfirm}`}
                        onClick={() => {
                          changeBookingStatusHadnler(2);
                        }}
                      />
                    </Tooltip>
                  </Grid>
                  <Grid
                    container
                    item
                    alignItems="center"
                    style={{ width: "auto" }}
                  >
                    <Tooltip title="Reject Booking" arrow>
                      <ThumbDownSharpIcon
                        className={`${css.thumbButtons} ${css.thumbReject}`}
                        onClick={() => {
                          changeBookingStatusHadnler(3);
                        }}
                      />
                    </Tooltip>
                  </Grid>
                </Grid>
              )}
              {booking.bookingStatus === "Rejected" && (
                <Typography className={css.state}>Rejected</Typography>
              )}
            </Grid>
          )}
        </Grid>
      </CardContent>

      <CardContent className={css.cardBody}>
        <Grid container justifyContent="space-around">
          <Grid container item direction="column" className={css.column}>
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: `${
                  booking.carWashShopName.length > 15 ? "flex-start" : "center"
                }`,
                margin: "1em 0",
              }}
            >
              <Grid item>
                <StorefrontIcon
                  className={css.icon}
                  style={{ top: "4px", position: "relative" }}
                />
              </Grid>
              <Grid container item direction="column" style={{ width: "auto" }}>
                <Typography className={css.infoType}>Shop</Typography>
                <Typography
                  className={css.infoText}
                  style={{
                    width: "150px",
                    fontSize: "16px",
                  }}
                >
                  {booking.carWashShopName}
                </Typography>
              </Grid>
            </Grid>
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: `${
                  booking.address.length > 15 ? "flex-start" : "center"
                }`,
                margin: "1em 0",
              }}
            >
              <Grid item>
                <RoomIcon
                  className={css.icon}
                  style={{ top: "4px", position: "relative" }}
                />
              </Grid>
              <Grid container item direction="column" style={{ width: "auto" }}>
                <Typography className={css.infoType}>Address</Typography>
                <Typography
                  className={css.infoText}
                  style={{
                    width: "150px",
                  }}
                >
                  {booking.address}
                </Typography>
              </Grid>
            </Grid>
          </Grid>
          <Grid container item direction="column" className={css.column}>
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: "center",
                margin: "1em 0",
              }}
            >
              <Grid item>
                <LocalCarWashIcon
                  className={css.icon}
                  style={{ top: "4px", position: "relative" }}
                />
              </Grid>
              <Grid container item direction="column" style={{ width: "auto" }}>
                <Typography className={css.infoType}>Service</Typography>
                <Typography className={css.infoText}>
                  {booking.serviceName}
                </Typography>
              </Grid>
            </Grid>
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: "center",
                margin: "1em 0",
              }}
            >
              <Grid item>
                <EventIcon
                  className={css.icon}
                  style={{ top: "4px", position: "relative" }}
                />
              </Grid>
              <Grid container item direction="column" style={{ width: "auto" }}>
                <Typography className={css.infoType}>Date</Typography>
                <Typography className={css.infoText}>
                  {booking.scheduledDate}
                </Typography>
              </Grid>
            </Grid>
          </Grid>
          <Grid container item direction="column" className={css.column}>
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: "center",
                margin: "1em 0",
              }}
            >
              <Grid item>
                <BookingStatusIcon
                  className={css.icon}
                  style={{ top: "4px", position: "relative" }}
                />
              </Grid>
              <Grid container item direction="column" style={{ width: "auto" }}>
                <Typography className={css.infoType}>Status</Typography>
                <Typography className={css.infoText}>
                  {booking.bookingStatus}
                </Typography>
              </Grid>
            </Grid>
            <Grid
              container
              item
              style={{
                width: "auto",
                alignItems: "center",
                margin: "1em 0",
              }}
            >
              <Grid item>
                <AccessTimeIcon
                  className={css.icon}
                  style={{ top: "4px", position: "relative" }}
                />
              </Grid>
              <Grid container item direction="column" style={{ width: "auto" }}>
                <Typography className={css.infoType}>Time</Typography>
                <Typography className={css.infoText}>
                  {booking.scheduledTime}
                </Typography>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </CardContent>
      {role === "Owner" && (
        <Grid container>
          <Button
            disableRipple
            className={`${css.collapseBtn} ${expanded && css.expandedBtn}`}
            onClick={() => {
              setExpanded(!expanded);
            }}
          >
            {expanded ? "COLLAPSE DETAILS" : "EXPAND DETAILS"}
          </Button>
          <Collapse
            component={Grid}
            container
            direction="column"
            classes={{
              wrapper: css.collapseWrapper,
            }}
            in={expanded}
            timeout="auto"
            unmountOnExit
          >
            <Grid container item>
              <Grid
                container
                item
                style={{
                  width: "auto",
                  alignItems: "center",
                }}
              >
                <PersonSharpIcon
                  style={{
                    color: "white",
                    fontSize: 315,
                  }}
                />
              </Grid>
              <Grid
                container
                item
                justifyContent="center"
                direction="column"
                style={{ width: "auto" }}
              >
                <Grid
                  container
                  item
                  style={{
                    width: "auto",
                    alignItems: "center",
                    margin: "0.5em 0",
                  }}
                >
                  <Grid item>
                    <AccountCircleSharpIcon
                      className={css.icon}
                      style={{ top: "4px", position: "relative" }}
                    />
                  </Grid>
                  <Grid
                    container
                    item
                    direction="column"
                    style={{ width: "auto" }}
                  >
                    <Typography className={css.infoType}>Username</Typography>
                    <Typography className={css.infoText}>
                      {booking.consumerUsername}
                    </Typography>
                  </Grid>
                </Grid>
                <Grid
                  container
                  item
                  style={{
                    width: "auto",
                    alignItems: "center",
                    margin: "0.5em 0",
                  }}
                >
                  <Grid item>
                    <PhoneEnabledSharpIcon
                      className={css.icon}
                      style={{ top: "4px", position: "relative" }}
                    />
                  </Grid>
                  <Grid
                    container
                    item
                    direction="column"
                    style={{ width: "auto" }}
                  >
                    <Typography className={css.infoType}>
                      Contact Phone
                    </Typography>
                    <Typography className={css.infoText}>
                      {booking.contactPhone}
                    </Typography>
                  </Grid>
                </Grid>
                <Grid
                  container
                  item
                  style={{
                    width: "auto",
                    alignItems: "center",
                    margin: "0.5em 0",
                  }}
                >
                  <Grid item>
                    <AlternateEmailSharpIcon
                      className={css.icon}
                      style={{ top: "4px", position: "relative" }}
                    />
                  </Grid>
                  <Grid
                    container
                    item
                    direction="column"
                    style={{ width: "auto" }}
                  >
                    <Typography className={css.infoType}>E-mail</Typography>
                    <Typography className={css.infoText}>
                      {booking.email}
                    </Typography>
                  </Grid>
                </Grid>
              </Grid>
            </Grid>
          </Collapse>
        </Grid>
      )}
      <CardContent className={css.cardFooter}>
        <Grid container item justifyContent="center" style={{ width: "auto" }}>
          <Grid container item style={{ width: "auto" }}>
            <Typography className={css.value} style={{ fontSize: 40 }}>
              $ {booking.price}
            </Typography>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  );
}
