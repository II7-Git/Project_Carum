import styles from "./Shop.module.css";
import TopNav from "components/TopNav";
import { useEffect, useState } from "react";
import Button from "components/Button";
import MenuIcon from "@material-ui/icons/Menu";
import SearchIcon from "@material-ui/icons/Search";
import Pagination from "@mui/material/Pagination";
import FurnitureComponent from "./FurnitureComponent";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import CloseIcon from "@material-ui/icons/Close";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import { furnitureCategory } from "utils/utils";
import { fetchShopItem, purchaseFurniture, fetchMyItem } from "apis/furniture";

function Shop() {
  const [place, setPlace] = useState("shop");
  const [isOpened, setIsOpened] = useState(false);
  const [categoryIndex, setCategoryIndex] = useState(0);
  const [page, setPage] = useState(1);
  const [totalPage, setTotalPage] = useState(1);
  const [money, setMoney] = useState(0);
  const [currentFurnitureIndex, setCurrentFurnitureIndex] = useState(0);
  const [anchorEl, setAnchorEl] = useState(null);
  const isMenuOpened = Boolean(anchorEl);
  const [searchText, setSearchText] = useState("");
  const [furnitureList, setFurnitureList] = useState([]);

  // 처음 가구 불러오기
  const fetchShopItemSuccess = (res) => {
    console.log(res.data);
    setFurnitureList(res.data.furnitureList);
    setMoney(res.data.money);

    let tmpTotalPage = parseInt(res.data.furnitureCount / 4);
    if (res.data.furnitureCount % 4) {
      tmpTotalPage += 1;
    }
    setTotalPage(tmpTotalPage);
  };

  const fetchShopItemFail = (err) => {
    console.log(err);
  };

  //페이지 이동 시, 카테고리 변경 시 가구 검색
  useEffect(() => {
    if (place === "shop") {
      const payload = {
        keyword: searchText ? searchText : null,
        type: furnitureCategory[categoryIndex].type,
        page: page - 1,
        size: 4,
      };

      fetchShopItem(payload, fetchShopItemSuccess, fetchShopItemFail);
    }
  }, [page]);

  useEffect(() => {
    setSearchText("");
    const payload = {
      keyword: null,
      type: furnitureCategory[categoryIndex].type,
      page: page - 1,
      size: 4,
    };

    fetchShopItem(payload, fetchShopItemSuccess, fetchShopItemFail);
  }, [categoryIndex]);

  const handleFurnitureSearch = () => {
    if (searchText) {
      const payload = {
        keyword: searchText,
        type: furnitureCategory[categoryIndex].type,
        page: 0,
        size: 4,
      };
      fetchShopItem(payload, fetchShopItemSuccess, fetchShopItemFail);
    }
  };

  const fetchMyItemSuccess = (res) => {
    console.log(res.data);
    setPage(1);
    setCategoryIndex(0);
    setFurnitureList(res.data.furnitureList);
  };

  const fetchMyItemFail = (err) => {
    console.log(err);
  };

  // 가구점, 내 가구 이동
  const movePlace = () => {
    if (place === "myRoom") {
      setPlace("shop");
      const payload = {
        keyword: null,
        type: furnitureCategory[categoryIndex].type,
        page: page - 1,
        size: 4,
      };

      fetchShopItem(payload, fetchShopItemSuccess, fetchShopItemFail);
    } else {
      setPlace("myRoom");
      fetchMyItem(fetchMyItemSuccess, fetchMyItemFail);
    }
  };

  // 가구 dialog 열기
  const handleOpen = (idx) => {
    if (place === "shop" && !furnitureList[idx].have) {
      setIsOpened(true);
      setCurrentFurnitureIndex(idx);
    }
  };

  // 가구 dialog 닫기
  const handleClose = () => {
    setIsOpened(false);
  };

  // 카테고리 메뉴 열고 닫기
  const handleMenuClick = (event, menu) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = (index) => {
    setAnchorEl(null);
    setCategoryIndex(index);
    setPage(1);
  };

  // 페이지 이동
  const handlePageChange = (e, p) => {
    setPage(p);
  };

  // 가구 구매
  const purchaseFurnitureSuccess = (res) => {
    console.log(res);
  };

  const purchaseFurnitureFail = (err) => {
    console.log(err);
  };

  const handlePurchaseFurniture = (furnitureId) => {
    purchaseFurniture(
      furnitureId,
      purchaseFurnitureSuccess,
      purchaseFurnitureFail
    );
  };

  return (
    <div>
      <TopNav
        text="가구점"
        buttonComponent={
          <Button
            variant="primary"
            size="extraSmall"
            text={place === "myRoom" ? "상점" : "내가구"}
            onClick={() => movePlace()}
          />
        }
      />
      {place === "shop" ? (
        <div>
          <div className={styles.searchBox}>
            <MenuIcon onClick={handleMenuClick} id="menu-bth" />
            <Menu
              anchorEl={anchorEl}
              open={isMenuOpened}
              onClose={handleMenuClose}
              MenuListProps={{ "aria-labelledby": "menu-btn" }}
              sx={{ maxHeight: "200px" }}
            >
              {furnitureCategory.map((el, idx) => {
                return (
                  <MenuItem onClick={() => handleMenuClose(idx)} key={idx}>
                    {el.name}
                  </MenuItem>
                );
              })}
            </Menu>
            <input
              className={styles.inputBox}
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
            />
            <SearchIcon onClick={handleFurnitureSearch} />
          </div>
          <div className={styles.contentBox}>
            <p className={styles.categoryName}>
              {furnitureCategory[categoryIndex].name}
            </p>
            <div className={styles.furnitureRow}>
              {furnitureList?.map((el, idx) => {
                if (idx < 2) {
                  return (
                    <FurnitureComponent
                      name={el.name}
                      have={el.have}
                      place={place}
                      point={el.price}
                      onClick={() => handleOpen(idx)}
                      key={idx}
                    />
                  );
                }
              })}
            </div>
            <div className={styles.furnitureRow}>
              {furnitureList?.map((el, idx) => {
                if (idx > 1) {
                  return (
                    <FurnitureComponent
                      name={el.name}
                      have={el.have}
                      place={place}
                      point={el.price}
                      onClick={() => handleOpen(idx)}
                      key={idx}
                    />
                  );
                }
              })}
            </div>
            <Pagination
              size="small"
              count={totalPage}
              className={styles.pagination}
              onChange={handlePageChange}
              defaultPage={1}
              page={page}
            />
          </div>
          {/* 가구 dialog */}
          <Dialog
            className={styles.dialog}
            open={isOpened}
            onClose={handleClose}
          >
            <div className={styles.dialog}>
              <CloseIcon onClick={handleClose} className={styles.closeBtn} />
              <DialogTitle>
                {furnitureList[currentFurnitureIndex]?.name}
              </DialogTitle>
              <div className={styles.furniture}>가구</div>
              <p className={styles.detailPriceTag}>
                {furnitureList[currentFurnitureIndex]?.price}
              </p>
              <div className={styles.myPointBox}>
                <p className={styles.myPointText}>내 포인트</p>
                <p className={styles.myPointText}>{money} point</p>
              </div>
              <Button
                text="구매"
                variant="light"
                size="small"
                onClick={() =>
                  handlePurchaseFurniture(
                    furnitureList[currentFurnitureIndex]?.id
                  )
                }
              />
            </div>
          </Dialog>
        </div>
      ) : (
        <MyRoomComponent furnitureList={furnitureList} place={place} />
      )}
    </div>
  );
}

function MyRoomComponent({ furnitureList, place }) {
  const [newPage, setNewPage] = useState(1);
  const [newTotalPage, setNewTotalPage] = useState(
    furnitureList.length ? furnitureList.length : 0
  );

  const handleInventoryPage = (e, p) => {
    setNewPage(p);
  };

  return (
    <div>
      {furnitureList.length > 0 ? (
        <div className={styles.contentBox}>
          <div className={styles.furnitureRow}>
            {furnitureList[(newPage - 1) * 4] ? (
              <FurnitureComponent
                name={furnitureList[(newPage - 1) * 4]?.name}
                place={place}
              />
            ) : null}
            {furnitureList[(newPage - 1) * 4 + 1] ? (
              <FurnitureComponent
                name={furnitureList[(newPage - 1) * 4 + 1]?.name}
                place={place}
              />
            ) : null}
          </div>
          <div className={styles.furnitureRow}>
            {furnitureList[(newPage - 1) * 4 + 2] ? (
              <FurnitureComponent
                name={furnitureList[(newPage - 1) * 4 + 2]?.name}
                place={place}
              />
            ) : null}
            {furnitureList[(newPage - 1) * 4 + 3] ? (
              <FurnitureComponent
                name={furnitureList[(newPage - 1) * 4 + 3]?.name}
                place={place}
              />
            ) : null}
          </div>
          <Pagination
            size="small"
            count={newTotalPage}
            className={styles.pagination}
            onChange={handleInventoryPage}
            defaultPage={1}
            page={newPage}
          />
        </div>
      ) : (
        <p className={styles.inventoryNoDataText}>가구가 없습니다.</p>
      )}
    </div>
  );
}

export default Shop;
