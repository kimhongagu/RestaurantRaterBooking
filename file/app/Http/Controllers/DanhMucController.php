<?php

namespace App\Http\Controllers;

use App\Models\DanhMuc;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class DanhMucController extends Controller
{
    /**
     * Display a listing of the resource.
     */
    public function getDanhSach()
    {
        $danhmuc = DanhMuc::all();
        return view('danhmuc.danhsach', compact('danhmuc'));
    }
    public function getThem()
    {
        return view('danhmuc.them');
    }
    public function postThem(Request $request)
    {
        $orm = new DanhMuc();
        $orm->tendanhmuc = $request->tendanhmuc;
        $orm->tendanhmuc_slug = Str::slug($request->tendanhmuc, '-');
        $orm->loai = $request->loai;
        $orm->save();
        return redirect()->route('danhmuc');
    }
    public function getSua($id)
    {
        $danhmuc = DanhMuc::find($id);
        return view('danhmuc.sua', compact('danhmuc'));
    }
    public function postSua(Request $request, $id)
    {
        $orm = DanhMuc::find($id);
        $orm->tendanhmuc = $request->tendanhmuc;
        $orm->tendanhmuc_slug = Str::slug($request->tendanhmuc, '-');
        $orm->loai = $request->loai;
        $orm->save();
        return redirect()->route('danhmuc');
    }
    public function getXoa($id)
    {
        $orm = DanhMuc::find($id);
        $orm->delete();
        return redirect()->route('danhmuc');
    }
}
