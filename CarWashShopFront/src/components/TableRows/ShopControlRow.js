import { makeStyles } from "@material-ui/core/styles";
import { useState, useRef } from "react";
import {
  Grid,
  Typography,
  Collapse,
  TextField,
  Tooltip,
} from "@material-ui/core";
import PeopleAltIcon from "@material-ui/icons/PeopleAlt";
import ThumbUpSharpIcon from "@material-ui/icons/ThumbUpSharp";
import BlockIcon from "@material-ui/icons/Block";
import CloseSharpIcon from "@material-ui/icons/CloseSharp";
import CoownerNameRepresentation from "../CoownerNameRepresentation";

const useStyles = makeStyles((theme) => ({
  cell: {
    justifyContent: "center",
    width: "25%",
  },
  info: {
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 24,
    textAlign: "center",
  },
  collapseWrapper: {
    width: "100%",
  },
  collapseGridContainer: {
    borderTop: "1px dashed dodgerblue",
    padding: "4rem",
    backgroundColor: "rgba(0,0,0,0.4)",
  },
  collapseCellRight: {
    width: "45%",
    flexDirection: "column",
    justifyItems: "center",
    justifyContent: "flex-start",
  },
  collapseCellLeft: {
    width: "55%",
    flexDirection: "column",
    justifyItems: "center",
    justifyContent: "center",
    color: "white",
  },
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
  textField: {
    "& .MuiOutlinedInput-root": {
      width: "20rem",
    },
    "& .MuiOutlinedInput-notchedOutline": {
      border: "3px dodgerblue solid",
    },
    "& .MuiInputLabel-outlined": {
      fontSize: 20,
    },
    "& .MuiOutlinedInput-input": {
      height: "1.5rem",
    },
  },
  textFieldTypography: {
    fontFamily: "Orbitron",
    bottom: "0.5rem",
    fontSize: 22,
    position: "relative",
    fontWeight: 500,
  },
  avatar: {
    fontSize: 200,
    border: "4px solid dodgerblue",
    borderRadius: "999px",
    padding: "1rem",
    marginBottom: "2rem",
  },
  thumbButtons: {
    fontSize: 50,
    borderRadius: "50px",
    transition: "0.12s linear",
    cursor: "pointer",
    transition: "0.15s linear",
    color: "whitesmoke",
  },
  thumbConfirm: {
    fontSize: 34,
    border: "0px transparent solid",
    "&:hover": {
      color: "limegreen",
    },
    "&:active": {
      transition: "0.05s linear",
      border: "8px transparent solid",
    },
  },
  disabledIcon: {
    fontSize: 50,
    padding: "0.1em",
    borderRadius: "50px",
    transition: "0.12s linear",
    cursor: "pointer",
    transition: "0.15s linear",
    color: "dodgerblue",
    filter: "drop-shadow(0 0 0.4rem dodgerblue)",
  },
  cancelRemovalBtn: {
    border: "0px transparent solid",
    "&:hover": {
      color: "red",
    },
    "&:active": {
      transition: "0.05s linear",
      border: "12px transparent solid",
    },
  },
  rowContainer: {
    padding: "0.8rem 0",
    borderTop: "2px solid dodgerblue",
    cursor: "pointer",
    alignItems: "center",
    "&:hover": {
      backgroundColor: "rgba(0,0,0,0.4)",
    },
  },
  bodyMessageText: {
    fontFamily: "Orbitron",
    fontSize: 28,
    fontWeight: 500,
    color: "orange",
    textAlign: "center",
  },
}));

const ShopControlRow = (props) => {
  const css = useStyles();
  const item = props.item;
  const currentUser = localStorage.getItem("userName");
  const newOwnerRef = useRef();
  const [expanded, setExpanded] = useState(false);

  const otherUsers = item.owners.filter((x) => x !== currentUser);

  const bodyMessaging = (body) => {
    const message = (
      <Grid container direction="column" alignItems="center">
        <Grid item className={css.bodyMessageText}>
          {body}
        </Grid>
        <Grid item className={css.bodyMessageText} style={{ fontSize: 40 }}>
          {"Are you sure?"}
        </Grid>
      </Grid>
    );

    return message;
  };

  const shopRemovalMsg = bodyMessaging(
    `Shop removal of the ${item.name} will be canceled.`
  );

  const shopApprovalMsg = bodyMessaging(
    `You will approve ${item.name} removal.`
  );

  const disbandApprovalMsg = bodyMessaging(
    `You will approve disband from ${item.name}, and abandon the shop.`
  );

  const newOwnerMsg = bodyMessaging(
    `You have added new owner to the ${item.name}`
  );
  return (
    <Grid container direction="column">
      <Grid
        container
        item
        className={css.rowContainer}
        onClick={() => {
          setExpanded(!expanded);
        }}
        style={{ backgroundColor: `${expanded ? "rgba(0,0,0,0.4)" : ""}` }}
      >
        <Grid container item className={css.cell} style={{ width: "35%" }}>
          <Typography className={css.info}>{item.name}</Typography>
        </Grid>
        <Grid container item className={css.cell} style={{ width: "20%" }}>
          <Typography className={css.info}>{item.owners.length - 1}</Typography>
        </Grid>
        <Grid
          container
          item
          className={css.cell}
          style={{ alignItems: "center", width: "20%" }}
        >
          {item.disband ? (
            <Tooltip title={`Abandon '${item.name}'`} arrow>
              <ThumbUpSharpIcon
                className={`${css.thumbButtons} ${css.thumbConfirm}`}
                onClick={(e) => {
                  e.stopPropagation();
                  props.prompt(
                    "disbandApproved",
                    "Disband Request",
                    disbandApprovalMsg,
                    item.name,
                    item.id,
                    null
                  );
                }}
              />
            </Tooltip>
          ) : (
            <BlockIcon className={css.disabledIcon} />
          )}
        </Grid>
        <Grid
          container
          item
          alignItems="center"
          className={css.cell}
          style={{ width: "25%" }}
        >
          {item.shopRemoval ? (
            <Tooltip title={"Approve"} arrow>
              <ThumbUpSharpIcon
                className={`${css.thumbButtons} ${css.thumbConfirm}`}
                style={{ marginRight: "1.5rem" }}
                onClick={(e) => {
                  e.stopPropagation();
                  props.prompt(
                    "removalApproved",
                    "Shop removal",
                    shopApprovalMsg,
                    item.name,
                    item.id,
                    null
                  );
                }}
              />
            </Tooltip>
          ) : (
            <BlockIcon
              className={css.disabledIcon}
              style={{
                marginRight: "1.5rem",
              }}
            />
          )}
          {item.isInRemovalProcess ? (
            <Tooltip title={"Cancel removal process"} arrow>
              <CloseSharpIcon
                className={`${css.thumbButtons} ${css.cancelRemovalBtn}`}
                onClick={(e) => {
                  e.stopPropagation();
                  props.prompt(
                    "removalCanceled",
                    "Shop removal",
                    shopRemovalMsg,
                    item.name,
                    item.id,
                    null
                  );
                }}
              />
            </Tooltip>
          ) : (
            <BlockIcon className={css.disabledIcon} />
          )}
        </Grid>
      </Grid>
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
        <Grid container className={css.collapseGridContainer}>
          <Grid container item className={css.collapseCellLeft}>
            <Grid
              container
              item
              direction="column"
              style={{
                width: "auto",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <PeopleAltIcon className={css.avatar} />
              <TextField
                type="text"
                inputRef={newOwnerRef}
                variant="outlined"
                label={
                  <Grid container item style={{ width: "9.8rem" }}>
                    <Typography className={css.textFieldTypography}>
                      +&nbsp;Add&nbsp;new&nbsp;owner
                    </Typography>
                  </Grid>
                }
                className={css.textField}
                onKeyDown={(e) => {
                  if (e.key === "Enter") {
                    const ownerName = newOwnerRef.current.value;
                    props.prompt(
                      "addNewOwner",
                      "New owner",
                      newOwnerMsg,
                      null,
                      null,
                      {
                        shopId: item.id,
                        ownerUserName: [ownerName],
                      }
                    );
                  }
                }}
              />
            </Grid>
          </Grid>
          <Grid container item className={css.collapseCellRight}>
            {otherUsers.length > 0 &&
              otherUsers.map((x) => (
                <CoownerNameRepresentation
                  key={x}
                  owner={x}
                  prompt={props.prompt}
                  shopId={item.id}
                  bodyMessaging={bodyMessaging}
                />
              ))}
          </Grid>
        </Grid>
      </Collapse>
    </Grid>
  );
};

export default ShopControlRow;
