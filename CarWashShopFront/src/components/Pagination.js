import { useState } from "react";
import TablePagination from "@material-ui/core/TablePagination";

export default function TablePaginationDemo(props) {
  const count = parseInt(props.totalCountOfItems);
  const handleChangePage = (event, newPage) => {
    props.setPagination((prevValue) => {
      return {
        ...prevValue,
        currentPage: newPage + 1,
      };
    });
  };
  const handleChangeRowsPerPage = (event) => {
    props.setPagination((prevValue) => {
      return { currentPage: 1, recordsPerPage: event.target.value };
    });
  };

  return (
    <TablePagination
      style={{ color: "white" }}
      component="div"
      count={count}
      page={parseInt(props.pagination.currentPage) - 1}
      onPageChange={handleChangePage}
      rowsPerPage={props.pagination.recordsPerPage}
      onRowsPerPageChange={handleChangeRowsPerPage}
    />
  );
}
