import styles from "./Profile.module.css";
import TopNav from "../../components/TopNav";
import Button from "../../components/Button";
import { TextField } from "@mui/material";
import { useCallback, useEffect, useState } from "react";
import EditIcon from "@material-ui/icons/Edit";
import { Button as MUIButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Dialog } from "@mui/material";
import { DialogTitle } from "@mui/material";
import {
  logout,
  deleteAccount,
  fetchProfile,
  checkValidNickname,
  editNickname,
  changePassword,
} from "apis/user";
import Swal from "sweetalert2";
import { preventRefresh, errorAlert, goToMain } from "utils/utils";
import { useAppDispatch } from "stores/store";
import { setUserInfo } from "stores/slices/user";
import { setNowRoomId } from "stores/slices/room";

function Profile() {
  const [values, setValues] = useState({
    userInfo: null,
    isEditing: false,
    newNickname: "",
    isValidNickname: null,
    oldPassword: "",
    isOldPasswordValid: null,
    newPassword: "",
    newPasswordConfirm: "",
    isDeleting: false,
  });

  const [nicknameErrorText, setNicknameErrorText] = useState("");

  const navigate = useNavigate();

  // redux
  const dispatch = useAppDispatch();

  const exitRoom = useCallback(() => {
    dispatch(setNowRoomId(null));
    dispatch(setUserInfo(null));
    localStorage.clear();
  }, [dispatch]);

  // 회원 정보 조회

  const fetchProfileSuccess = (res) => {
    console.log(res);
    const userInfo = {
      nickname: res.data.nickName,
      id: res.data.userId,
      birth: res.data.birth,
      phone: res.data.phone,
      money: res.data.money,
      mainRoom: res.data.mainRoom,
    };
    setValues({ ...values, userInfo, newNickname: res.data.nickName });
  };

  const fetchProfileFail = (err) => {
    console.log(err);
    errorAlert("회원 정보를 불러오지 못했어요 ㅠㅠ");
    navigate("/");
  };

  useEffect(() => {
    fetchProfile(fetchProfileSuccess, fetchProfileFail);
  }, []);

  // 닉네임 중복 확인
  const handleValidCheck = () => {
    if (
      values.userInfo.nickname !== values.newNickname &&
      values.newNickname !== ""
    ) {
      checkValidNickname(
        values.newNickname,
        () => {
          setValues({ ...values, isValidNickname: true });
          setNicknameErrorText("사용 가능한 닉네임입니다.");
        },
        () => {
          setValues({ ...values, isValidNickname: false });
          setNicknameErrorText("중복된 닉네임입니다.");
        }
      );
    } else if (values.newNickname === "") {
      setNicknameErrorText("닉네임은 필수로 입력해야 합니다");
      setValues({ ...values, isValidNickname: false });
    }
  };

  // 닉네임 수정
  const editNicknameSuccess = (res) => {
    console.log("수정수정");
    const newUserInfo = values.userInfo;
    newUserInfo.nickname = values.newNickname;
    setValues({
      ...values,
      userInfo: newUserInfo,
      isEditing: false,
      isValidNickname: null,
    });
    setNicknameErrorText("");
  };

  const editNicknameFail = (err) => {
    console.log(err);
  };

  const handleEditNickname = (state) => {
    if (state === "edit") {
      if (values.newNickname === values.userInfo.nickname) {
        setValues({
          ...values,
          isEditing: false,
          isValidNickname: null,
          newNickname: values.userInfo.nickname,
        });
        setNicknameErrorText("");
      } else if (values.isValidNickname !== true) {
        Swal.fire({
          icon: "error",
          title: "중복 확인을 받아주세요",
          showConfirmButton: false,
          timer: 800,
        });
      } else {
        editNickname(values.newNickname, editNicknameSuccess, editNicknameFail);
      }
    } else {
      setValues({
        ...values,
        isEditing: false,
        isValidNickname: null,
        newNickname: values.userInfo.nickname,
      });
      setNicknameErrorText("");
    }
  };

  // 비밀번호 수정
  const changePasswordSuccess = (res) => {
    setValues({
      ...values,
      oldPassword: "",
      newPassword: "",
      newPasswordConfirm: "",
      isOldPasswordValid: null,
    });
    Swal.fire({
      position: "center",
      icon: "success",
      title: "비밀번호가 변경되었습니다.",
      showConfirmButton: false,
      timer: 1000,
    });
  };

  const changePasswordFail = (err) => {
    console.log(err);
    setValues({ ...values, isOldPasswordValid: false });
  };

  const handleChangePassword = () => {
    if (
      values.oldPassword &&
      values.newPassword &&
      values.newPassword === values.newPasswordConfirm
    ) {
      const payload = {
        oldPassword: values.oldPassword,
        newPassword: values.newPassword,
      };
      changePassword(payload, changePasswordSuccess, changePasswordFail);
    }
  };

  // 로그아웃
  const logoutSuccess = (res) => {
    sessionStorage.clear();
    exitRoom();
    navigate("/login");
  };

  const logoutFail = (err) => {
    console.log(err);
  };

  const handleLogout = () => {
    logout(logoutSuccess, logoutFail);
  };

  // 회원 탈퇴
  const deleteAccountSuccess = (res) => {
    console.log(res);
    sessionStorage.removeItem("access-token");
    sessionStorage.removeItem("refresh-token");
    navigate("/");
  };

  const deleteAccountFail = (err) => {
    console.log(err);
  };
  const handleDeleteAccount = () => {
    deleteAccount(deleteAccountSuccess, deleteAccountFail);
  };

  // 회원 탈퇴 dialog 열고 닫기
  const deleteDialogOpen = () => {
    setValues({ ...values, isDeleting: true });
  };

  const deleteDialogClose = () => {
    setValues({ ...values, isDeleting: false });
  };

  // 새로고침 방지
  useEffect(() => {
    window.addEventListener("beforeunload", preventRefresh);

    goToMain();
  }, []);

  return (
    <div className={styles.box}>
      <TopNav text="내 정보" />
      <div className={styles.container}>
        <p className={styles.id}>{values.userInfo?.id}님</p>
        <p className={styles.settingTag}>닉네임</p>
        {values.isEditing ? (
          <div className={styles.nicknameEditbox}>
            <div className={styles.editRow}>
              <TextField
                helperText={nicknameErrorText}
                error={
                  values.isValidNickname === true
                    ? false
                    : values.isValidNickname === false
                    ? true
                    : null
                }
                size="small"
                value={values.newNickname}
                onChange={(e) =>
                  setValues({ ...values, newNickname: e.target.value })
                }
              />
              <button onClick={handleValidCheck} className={styles.validButton}>
                중복 확인
              </button>
            </div>
            <div className={styles.editButton}>
              <MUIButton
                onClick={() => handleEditNickname("close")}
                size="small"
                variant="contained"
                color="error"
              >
                취소
              </MUIButton>
              <MUIButton
                onClick={() => handleEditNickname("edit")}
                size="small"
                variant="contained"
                color="primary"
                disabled={
                  !(values.isValidNickname && values.newNickname !== "")
                }
              >
                수정
              </MUIButton>
            </div>
          </div>
        ) : (
          <div className={styles.nicknameSettingBox}>
            <p className={styles.nickname}>{values.userInfo?.nickname}</p>
            <EditIcon
              onClick={() => setValues({ ...values, isEditing: true })}
            />
          </div>
        )}
        <div>
          <p className={styles.settingTag}>이전 비밀번호</p>
          <TextField
            onChange={(e) =>
              setValues({ ...values, oldPassword: e.target.value })
            }
            size="small"
            className={styles.inputBox}
            type="password"
            value={values.oldPassword}
            error={values.isOldPasswordValid === false}
            helperText={
              values.isOldPasswordValid === false
                ? "비밀번호가 틀렸습니다."
                : null
            }
          />
        </div>
        <div>
          <p className={styles.settingTag}>새 비밀번호</p>
          <TextField
            onChange={(e) =>
              setValues({ ...values, newPassword: e.target.value })
            }
            size="small"
            className={styles.inputBox}
            type="password"
            value={values.newPassword}
            error={Boolean(
              values.oldPassword &&
                values.newPassword &&
                values.newPassword === values.oldPassword
            )}
            helperText={
              values.oldPassword &&
              values.newPassword &&
              values.newPassword === values.oldPassword
                ? "이전 비밀번호와 같습니다."
                : null
            }
          />
        </div>
        <div>
          <p className={styles.settingTag}>새 비밀번호 확인</p>
          <TextField
            onChange={(e) =>
              setValues({ ...values, newPasswordConfirm: e.target.value })
            }
            size="small"
            className={styles.inputBox}
            type="password"
            value={values.newPasswordConfirm}
            error={Boolean(
              values.newPassword &&
                values.newPasswordConfirm &&
                values.newPassword !== values.newPasswordConfirm
            )}
            helperText={
              values.newPassword &&
              values.newPasswordConfirm &&
              values.newPassword !== values.newPasswordConfirm
                ? "비밀번호가 다릅니다"
                : null
            }
          />
        </div>
        <div className={styles.editButton}>
          <div></div>
          <Button
            text="비밀번호 수정"
            variant="primary"
            size="small"
            onClick={() => handleChangePassword()}
          />
        </div>
        <div className={styles.buttonBox}>
          <MUIButton
            onClick={deleteDialogOpen}
            variant="contained"
            color="error"
          >
            회원탈퇴
          </MUIButton>
          <MUIButton onClick={handleLogout} variant="contained" color="info">
            로그아웃
          </MUIButton>
        </div>
      </div>
      <Dialog open={values.isDeleting}>
        <div className={styles.deleteDialog}>
          <DialogTitle>Carum에서 탈퇴하시겠습니까?</DialogTitle>
          <div className={styles.dialogBtnBox}>
            <MUIButton
              variant="contained"
              onClick={handleDeleteAccount}
              color="error"
            >
              예
            </MUIButton>
            <MUIButton variant="contained" onClick={deleteDialogClose}>
              아니요
            </MUIButton>
          </div>
        </div>
      </Dialog>
    </div>
  );
}

export default Profile;
