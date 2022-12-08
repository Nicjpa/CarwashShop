import { useState, useEffect } from "react";

import ConsumerShopFilter from "../components/Filters/ShopFilter";
import ConsumerShopTable from "../components/DataTables/ConsumerShopTable";
import { HTTPRequest } from "../HTTPRequest";

import { Grid, Zoom } from "@material-ui/core";

import LoadingModal from "../UI/Modals/LoadingModal";
import InfoModal from "../UI/Modals/InfoModal";
import NewBookingModal from "../UI/Modals/NewBookingModal";
import OwnerShopTable from "../components/DataTables/OwnerShopTable";
import PromptModal from "../UI/Modals/PromptModal";

const ShopPage = (props) => {
  const role = props.role;
  const [filterParams, setFilterParams] = useState(
    "&MinimumAmountOfWashingUnits=1&RequiredAndEarlierOpeningTime=1&RequiredAndLaterClosingTime=24"
  );

  const [reloadPage, setReloadPage] = useState(false);
  const [allShops, setAllShops] = useState({
    data: [],
    revenue: [],
    numOfPages: 0,
    totalCountOfItems: 0,
  });

  const [pagination, setPaginations] = useState({
    currentPage: 1,
    recordsPerPage: 10,
  });

  const [showLoading, setShowLoading] = useState(true);
  const [BookingModal, setBookingModal] = useState({
    bool: false,
    title: "Create booking",
    shopId: "",
    serviceId: "",
    bookingId: "",
    shopName: "",
    serviceName: "",
    servicePrice: "",
  });

  const [infoModalParams, setInfoModalParams] = useState({
    bool: false,
    modalTitle: "",
    modalDesc: [],
  });

  const [deleteServiceModal, setDeleteServiceModal] = useState({
    bool: false,
    title: "Delete service?",
    body: null,
    deleteFunc: null,
  });

  const reloadPageHandle = () => {
    setReloadPage(!reloadPage);
  };

  const createBookingHandle = async (scheduledDateTime) => {
    const httpParams = {
      controller: "ConsumerManagement/",
      action: "ScheduleAService",
      method: "POST",
      params: null,
      body: JSON.stringify({
        carWashShopId: BookingModal.shopId,
        serviceId: BookingModal.serviceId,
        scheduledDate: scheduledDateTime.date,
        scheduledHour: scheduledDateTime.time,
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    try {
      setShowLoading(true);
      const response = await HTTPRequest(httpParams);
      setBookingModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "New booking",
          modalDesc: [
            `BOOKING ID: #${response.data.id}`,
            `SERVICE NAME: "${response.data.serviceName}"`,
            `DATE: ${response.data.scheduledDate}`,
            `TIME: ${response.data.scheduledTime}`,
            `PRICE: $${response.data.price}`,
          ],
          themeColor: "info",
        };
      });
    } catch (error) {
      let errorMessage =
        error.message === "Failed to fetch"
          ? "No server response"
          : error.message.substring(0, 6) === "split,"
          ? error.message.substring(6, error.message.length - 1).split("*")
          : error.message;

      setBookingModal((prevValue) => {
        return { ...prevValue, bool: false };
      });

      setInfoModalParams(() => {
        return {
          bool: true,
          modalTitle: "Failed to book",
          modalDesc: errorMessage,
          themeColor: "error",
        };
      });
    }
    setShowLoading(false);
  };

  useEffect(() => {
    const consumerHttpParams = {
      controller: "ConsumerManagement/",
      action: `GetAllShops-ConsumerSide?Page=${pagination.currentPage}&RecordsPerPage=${pagination.recordsPerPage}`,
      method: "GET",
      params: filterParams,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const ownerShopHttpParams = {
      controller: "OwnerShops/",
      action: `GetAllShops-OwnersSide?Page=${pagination.currentPage}&RecordsPerPage=${pagination.recordsPerPage}`,
      method: "GET",
      params: filterParams,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const ownerRevenueHttpParams = {
      controller: "OwnerManagement/",
      action: `GetRevenue?Page=${pagination.currentPage}&RecordsPerPage=${pagination.recordsPerPage}`,
      method: "GET",
      params: filterParams,
      body: null,
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    };

    const httpParams =
      role === "Consumer" ? consumerHttpParams : ownerShopHttpParams;

    const httpCall = async () => {
      try {
        setShowLoading(true);
        const shopResponse = await HTTPRequest(httpParams);

        if (role === "Owner") {
          const revenueResponse = await HTTPRequest(ownerRevenueHttpParams);
          setAllShops(() => {
            return { ...shopResponse, revenue: revenueResponse };
          });
        } else {
          setAllShops(() => {
            return shopResponse;
          });
        }
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
            : "Shoplist error";

        setInfoModalParams(() => {
          return {
            bool: true,
            modalTitle: modalTitle,
            modalDesc: errorMessage,
            themeColor: "error",
          };
        });
      }

      setTimeout(() => {
        setShowLoading(false);
      }, 400);
    };

    httpCall();
  }, [filterParams, pagination, reloadPage, role]);

  return (
    <Grid container justifyContent="center">
      <LoadingModal loading={showLoading} />
      {!BookingModal.bool && !showLoading && (
        <InfoModal params={infoModalParams} setModalBool={setInfoModalParams} />
      )}
      {!infoModalParams.bool && !showLoading && (
        <NewBookingModal
          promptModal={BookingModal}
          closeModal={setBookingModal}
          createBookingHandle={createBookingHandle}
        />
      )}
      {!infoModalParams.bool && !showLoading && (
        <PromptModal
          promptModal={deleteServiceModal}
          closeModal={setDeleteServiceModal}
          executeYes={deleteServiceModal.deleteFunc}
        />
      )}
      <Grid container style={{ padding: "0 6em 3.5em 6em", width: "1920px" }}>
        <Zoom
          in={true}
          timeout={500}
          style={{
            transitionDelay: "300ms",
          }}
        >
          <Grid container item>
            <ConsumerShopFilter setFilterParams={setFilterParams} />
          </Grid>
        </Zoom>

        <Zoom
          in={true}
          timeout={500}
          style={{
            transitionDelay: "120ms",
          }}
        >
          <Grid
            container
            justifyContent="center"
            style={{
              margin: "1em 0",
              padding: "1em",
              borderRadius: "24px",
            }}
          >
            {role === "Consumer" ? (
              <ConsumerShopTable
                totalCountOfItems={allShops.totalCountOfItems}
                pagination={pagination}
                setPaginations={setPaginations}
                shops={allShops}
                setPromptModal={setBookingModal}
              />
            ) : (
              <OwnerShopTable
                totalCountOfItems={allShops.totalCountOfItems}
                pagination={pagination}
                setPaginations={setPaginations}
                shops={allShops}
                setPromptModal={setDeleteServiceModal}
                reloadPage={reloadPageHandle}
                setInfoModalParams={setInfoModalParams}
              />
            )}
          </Grid>
        </Zoom>
        {/* )} */}
      </Grid>
    </Grid>
  );
};

export default ShopPage;
