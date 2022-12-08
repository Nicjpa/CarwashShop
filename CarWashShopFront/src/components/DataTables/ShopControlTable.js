import { makeStyles } from "@material-ui/core/styles";
import { useEffect, useState } from "react";
import { HTTPRequest } from "../../HTTPRequest";
import { Grid, Typography, Zoom } from "@material-ui/core";

import StorefrontIcon from "@material-ui/icons/Storefront";
import PeopleAltIcon from "@material-ui/icons/PeopleAlt";
import DirectionsRunIcon from "@material-ui/icons/DirectionsRun";
import ErrorOutlineIcon from "@material-ui/icons/ErrorOutline";

import ShopControlRow from "../TableRows/ShopControlRow";
import Pagination from "../Pagination";

import LoadingModal from "../../UI/Modals/LoadingModal";
import InfoModal from "../../UI/Modals/InfoModal";
import PromptModal from "../../UI/Modals/PromptModal";

const useStyles = makeStyles((theme) => ({
  gridHeader: {
    padding: "1rem 0",
  },

  cellHead: {
    width: "25%",
    flexDirection: "column",
    alignItems: "center",
  },

  typographyCellHead: {
    color: "dodgerblue",
    fontFamily: "Orbitron",
    fontSize: 20,
    fontWeight: 500,
    textShadow: "0 0 10px dodgerblue",
  },
  iconCellHead: {
    fontSize: 50,
    color: "white",
    textShadow: "0 0 15px dodgerblue",
    filter: "drop-shadow(0 0 0.5rem dodgerblue)",
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
  },
  noContentSubtitle: {
    borderTop: "4px solid dodgerblue",
    paddingTop: "0.4rem",
    width: "auto",
    color: "white",
    fontFamily: "Orbitron",
    fontSize: 20,
    fontWeight: 700,
    textShadow: "0 0 3px dodgerblue",
    letterSpacing: "0.5em",
    textAlign: "center",
  },
  paginationGrid: {
    borderTop: "2px solid dodgerblue",
    padding: "0.5rem",
  },
}));

const ShopControlTable = () => {
  const css = useStyles();
  const [reloadPage, setReloadPage] = useState(false);
  const [promptModal, setPromptModal] = useState({
    action: null,
    bool: false,
    title: "",
    body: "",
    id: null,
    shopName: null,
    stringify: null,
  });

  const [infoModalParams, setInfoModalParams] = useState({
    bool: false,
    modalTitle: "",
    modalDesc: [],
  });

  const [dataTableInfo, setDataTableInfo] = useState({
    data: [],
    numOfPages: "",
    totalCountOfItems: "",
  });

  const [pagination, setPaginations] = useState({
    currentPage: 1,
    recordsPerPage: 10,
  });

  const filterResponse = (data) => {
    const filteredData = data.filter((x) => !x.isApproved);
    const shopIDs = filteredData.map((x) => x.carWashShopId);
    return shopIDs;
  };

  const httpAction = async (httpParams, successModalInfo, failedModalTitle) => {
    try {
      setPromptModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      await HTTPRequest(httpParams);

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: successModalInfo.title,
          modalDesc: successModalInfo.body,
          themeColor: "info",
        };
      });
      setReloadPage(!reloadPage);
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      setPromptModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: failedModalTitle,
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
  };

  const httpCall = async () => {
    try {
      const httpOwnerParams = {
        controller: "OwnerManagement/",
        action: `GetShopOwners?Page=${pagination.currentPage}&RecordsPerPage=${pagination.recordsPerPage}`,
        method: "GET",
        params: null,
        body: null,
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      };

      const httpDisbandParams = {
        controller: "OwnerManagement/",
        action: "GetDisbandRequests",
        method: "GET",
        params: null,
        body: null,
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      };

      const httpShopRemovalParams = {
        controller: "OwnerManagement/",
        action: "GetShopRemovalRequests",
        method: "GET",
        params: null,
        body: null,
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      };
      const ownerListResponse = await HTTPRequest(httpOwnerParams);
      const disbandReqsResponse = await HTTPRequest(httpDisbandParams);
      const shopRemovalsResponse = await HTTPRequest(httpShopRemovalParams);

      const noOwnerList = !!ownerListResponse.data.value;
      const noDisbandRequests = !!disbandReqsResponse.data.value;
      const noShopRemovals = !!shopRemovalsResponse.data.value;

      let disbandReqsFiltered = [];
      let shopRemovalsFiltered = [];
      let completeTableData = [];

      if (!noOwnerList) {
        completeTableData = ownerListResponse.data.map((x) => {
          return { ...x, disband: false, shopRemoval: false };
        });
      }

      if (!noDisbandRequests) {
        disbandReqsFiltered = filterResponse(disbandReqsResponse.data);
      }

      if (!noShopRemovals) {
        shopRemovalsFiltered = filterResponse(shopRemovalsResponse.data);
      }

      completeTableData.forEach((x) => {
        if (disbandReqsFiltered.includes(x.id)) {
          x.disband = true;
        }
        if (shopRemovalsFiltered.includes(x.id)) {
          x.shopRemoval = true;
        }
      });

      setDataTableInfo(() => {
        return {
          data: completeTableData,
          numOfPages: ownerListResponse.numOfPages,
          totalCountOfItems: ownerListResponse.totalCountOfItems,
        };
      });
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      setPromptModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Failed to load",
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
  };

  useEffect(() => {
    httpCall();
  }, [pagination, reloadPage]);

  const shopRemovalCanceled = () => {
    const httpParams = {
      controller: "OwnerManagement/",
      action: "CancelShopRemoval",
      method: "DELETE",
      params: `?shopID=${promptModal.id}`,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const successModalInfo = {
      title: "Shop removal",
      body: `You have canceled shop removal for '${promptModal.shopName}'.`,
    };

    httpAction(httpParams, successModalInfo, "Failed to cancel");
  };

  const shopRemovalApproved = () => {
    const httpParams = {
      controller: "OwnerManagement/",
      action: "ApproveShopRemoval",
      method: "PUT",
      params: `?shopID=${promptModal.id}`,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const successModalInfo = {
      title: "Shop removal",
      body: `You have approved shop removal for '${promptModal.shopName}'.`,
    };

    httpAction(httpParams, successModalInfo, "Failed to approve");
  };

  const disbandApproved = () => {
    const httpParams = {
      controller: "OwnerManagement/",
      action: "ApproveDisband",
      method: "DELETE",
      params: `?shopID=${promptModal.id}`,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const successModalInfo = {
      title: "Disband Request",
      body: `You have been disbanded from '${promptModal.shopName}'.`,
    };

    httpAction(httpParams, successModalInfo, "Failed to disband");
  };

  const requestDisband = (body) => {
    const httpParams = {
      controller: "OwnerManagement/",
      action: "RequestOwnerRemoval",
      method: "POST",
      params: null,
      body: JSON.stringify({
        ...body,
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const successModalInfo = {
      title: "Disband Request",
      body: `Disband request has been sent to '${body.ownerName}'.`,
    };

    httpAction(httpParams, successModalInfo, "Disband already exist");
  };

  const prompt = (action, title, body, shopName, id, stringify) => {
    setPromptModal(() => {
      return {
        action: action,
        title: title,
        body: body,
        shopName: shopName,
        id: id,
        bool: true,
        stringify: stringify,
      };
    });
  };

  const addNewOwner = (body) => {
    const httpParams = {
      controller: "OwnerManagement/",
      action: "AddOwnerToShop",
      method: "POST",
      params: null,
      body: JSON.stringify({
        ...body,
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const successModalInfo = {
      title: "New owner",
      body: `You have added '${body.ownerUserName}' as the new co-owner.`,
    };

    httpAction(httpParams, successModalInfo, "Add owner failed");
  };

  let action;

  switch (promptModal.action) {
    case "removalCanceled":
      action = shopRemovalCanceled;
      break;
    case "removalApproved":
      action = shopRemovalApproved;
      break;
    case "disbandApproved":
      action = disbandApproved;
      break;
    case "disbandRequest":
      action = () => {
        requestDisband(promptModal.stringify);
      };
      break;
    case "addNewOwner":
      action = () => {
        addNewOwner(promptModal.stringify);
      };
      break;
    default:
      break;
  }

  return (
    <>
      {!promptModal.bool && (
        <InfoModal params={infoModalParams} setModalBool={setInfoModalParams} />
      )}

      {!infoModalParams.bool && (
        <PromptModal
          promptModal={promptModal}
          closeModal={setPromptModal}
          executeYes={action}
        />
      )}
      <Zoom
        in={true}
        timeout={500}
        style={{
          transitionDelay: "200ms",
        }}
      >
        <Grid container>
          <Grid container item>
            {dataTableInfo.data.length > 0 && (
              <>
                <Grid container item className={css.gridHeader}>
                  <Grid
                    container
                    item
                    className={css.cellHead}
                    style={{ width: "35%" }}
                  >
                    <StorefrontIcon className={css.iconCellHead} />
                    <Typography className={css.typographyCellHead}>
                      Shop Name
                    </Typography>
                  </Grid>
                  <Grid
                    container
                    item
                    className={css.cellHead}
                    style={{ width: "20%" }}
                  >
                    <PeopleAltIcon className={css.iconCellHead} />
                    <Typography className={css.typographyCellHead}>
                      Co-owner Count
                    </Typography>
                  </Grid>
                  <Grid
                    container
                    item
                    className={css.cellHead}
                    style={{ width: "20%" }}
                  >
                    <DirectionsRunIcon className={css.iconCellHead} />
                    <Typography className={css.typographyCellHead}>
                      Disband Request
                    </Typography>
                  </Grid>
                  <Grid
                    container
                    item
                    className={css.cellHead}
                    style={{ width: "25%" }}
                  >
                    <ErrorOutlineIcon className={css.iconCellHead} />
                    <Typography className={css.typographyCellHead}>
                      Shop Removal
                    </Typography>
                  </Grid>
                </Grid>
                <Grid container item justifyContent="center">
                  {dataTableInfo.data.map((x, i) => (
                    <ShopControlRow key={i} item={x} prompt={prompt} />
                  ))}
                  <Grid
                    container
                    item
                    justifyContent="center"
                    className={css.paginationGrid}
                  >
                    <Grid item>
                      <Pagination
                        totalCountOfItems={dataTableInfo.totalCountOfItems}
                        pagination={pagination}
                        setPagination={setPaginations}
                      />
                    </Grid>
                  </Grid>
                </Grid>
              </>
            )}
            {dataTableInfo.data.length < 1 && (
              <Grid container item className={css.noContentContainer}>
                <Grid container item className={css.noContentTitle}>
                  NO SHOPS FOUND
                </Grid>
                <Grid item className={css.noContentSubtitle}>
                  CREATE YOUR FIRST SHOP
                </Grid>
              </Grid>
            )}
          </Grid>
        </Grid>
      </Zoom>
    </>
  );
};
export default ShopControlTable;
